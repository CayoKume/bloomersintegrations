using BloomersWorkers.ChangingOrder.Domain.Entities;
using BloomersWorkers.ChangingOrder.Infrastructure.Repositorys;
using BloomersWorkers.ChangingOrder.Infrastructure.Source.Pages;
using BloomersWorkersCore.Domain.CustomExceptions;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Infrastructure.Source.Drivers;
using BloomersWorkersCore.Infrastructure.Source.Pages;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;

namespace BloomersWorkers.ChangingOrder.Application.Services
{
    public class ChangingOrderService : IChangingOrderService
    {
        private readonly ILoginPage _loginPage;
        private readonly IHomePage _homePage;
        private readonly IChangingOrderPage _changingOrderPage;
        private readonly IChromeDriver _chromeDriver;
        private readonly IChangingOrderRepository _changingOrderRepository;
        private readonly IConfiguration _configuration;

        public ChangingOrderService (ILoginPage loginPage, IHomePage homePage, IChangingOrderPage changingOrderPage, IChromeDriver chromeDriver, IChangingOrderRepository changingOrderRepository, IConfiguration configuration) =>
            (_loginPage, _homePage, _changingOrderPage, _chromeDriver, _changingOrderRepository, _configuration) = (loginPage, homePage, changingOrderPage, chromeDriver, changingOrderRepository, configuration);

        public async Task ChangingOrder()
        {
            try
            {
                string? workerName = _configuration.GetSection("ConfigureService").GetSection("WorkerName").Value;
                var orders = await _changingOrderRepository.GetOrdersFromIT4();
                
                if (orders.Count() > 0)
                {
                    using (var driver = _chromeDriver.GetChromeDriverInstance())
                    {
                        var wait = _chromeDriver.GetWebDriverWaitInstance(driver);

                        foreach (var order in orders)
                        {
                            try
                            {
                                var user = await _changingOrderRepository.GetMicrovixUser(workerName);
                                _loginPage.InsertLoginAndPassword(user, wait);
                                _loginPage.SelectCompany(order.company.doc_company, user, driver, wait);

                                if (ChangingOrder(order, driver, wait))
                                    Log.Information($"Pedido: {order.number}, alterado com sucesso");
                            }
                            catch (CustomNoSuchElementException ex)
                            {
                                if (ex.pages == Page.TypeEnum.Login)
                                    Log.Warning("Errors => {@order} - {@ex}", order.number, ex.Message);
                                else if (ex.pages == Page.TypeEnum.Home)
                                    Log.Warning("Errors => {@order} - {@ex}", order.number, ex.Message);
                                else
                                    Log.Warning("Warnings => {@order} - {@ex}", order.number, ex.Message);

                                continue;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"{order.number} - {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Errors => {@ex}", ex.Message);
            }
        }

        private bool ChangingOrder(Order order, IWebDriver driver, WebDriverWait wait)
        {
            try
            {
                _homePage.ClosePendingInvoicesModal(driver, wait);
                _homePage.OpenSideMenu(driver, wait);
                _homePage.NavigateToChangingOrdersScreen(driver, wait);
                _changingOrderPage.SelectOrder(order.number, driver, wait);

                for (int i = 0; i < order.itens.Count(); i++)
                {
                    if (order.itens[i].qtde_volo == 0)
                        _changingOrderPage.RemoveItemFromList(order.itens[i].cod_product_volo, driver, wait);
                    else if (order.itens[i].qtde_volo != order.itens[i].qtde_microvix)
                        _changingOrderPage.ChangeQtdeItemFromList(order.itens[i].cod_product_volo, order.itens[i].qtde_volo, driver, wait);
                }
                return _changingOrderPage.AproveOrder(driver, wait);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
