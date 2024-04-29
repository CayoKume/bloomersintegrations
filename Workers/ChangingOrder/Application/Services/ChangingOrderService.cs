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
        private readonly IDriver _chromeDriver;
        private readonly IChangingOrderRepository _changingOrderRepository;
        private readonly IConfiguration _configuration;

        public ChangingOrderService (ILoginPage loginPage, IHomePage homePage, IChangingOrderPage changingOrderPage, IDriver chromeDriver, IChangingOrderRepository changingOrderRepository, IConfiguration configuration) =>
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
                        var user = await _changingOrderRepository.GetMicrovixUser(workerName);

                        for (int i = 0; i < orders.Count(); i++)
                        {
                            try
                            {
                                if (i == 0)
                                {
                                    _loginPage.InsertLoginAndPassword(user, wait);
                                    _loginPage.SelectCompany(orders[i].company.doc_company, driver, wait);
                                }
                                else
                                    _loginPage.SelectCompanyFromTopBar(orders[i].company.doc_company, wait);

                                if (ChangingOrder(orders[i], driver, wait))
                                {
                                    await _changingOrderRepository.UpdateReturnIT4ITEM(orders[i].number, orders[i].idControl);
                                    Log.Information($"Pedido: {orders[i].number}, alterado com sucesso");
                                }
                            }
                            catch (CustomNoSuchElementException ex)
                            {
                                if (ex.pages == Page.TypeEnum.Login)
                                    Log.Warning("Errors => {@order} - {@ex}", orders[i].number, ex.Message);
                                else if (ex.pages == Page.TypeEnum.Home)
                                    Log.Warning("Errors => {@order} - {@ex}", orders[i].number, ex.Message);
                                else
                                    Log.Warning("Warnings => {@order} - {@ex}", orders[i].number, ex.Message);

                                continue;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"{orders[i].number} - {ex.Message}");
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

        public async Task ChangingOrder(string workerName)
        {
            try
            {
                var orders = await _changingOrderRepository.GetOrdersFromIT4();

                if (orders.Count() > 0)
                {
                    using (var driver = _chromeDriver.GetChromeDriverInstance())
                    {
                        var wait = _chromeDriver.GetWebDriverWaitInstance(driver);
                        var user = await _changingOrderRepository.GetMicrovixUser(workerName);

                        for (int i = 0; i < orders.Count(); i++)
                        {
                            try
                            {
                                if (i == 0)
                                {
                                    _loginPage.InsertLoginAndPassword(user, wait);
                                    _loginPage.SelectCompany(orders[i].company.doc_company, driver, wait);
                                }
                                else
                                    _loginPage.SelectCompanyFromTopBar(orders[i].company.doc_company, wait);

                                if (ChangingOrder(orders[i], driver, wait))
                                {
                                    await _changingOrderRepository.UpdateReturnIT4ITEM(orders[i].number, orders[i].idControl);
                                    Log.Information($"Pedido: {orders[i].number}, alterado com sucesso");
                                }
                            }
                            catch (CustomNoSuchElementException ex)
                            {
                                if (ex.pages == Page.TypeEnum.Login)
                                    Log.Warning("Errors => {@order} - {@ex}", orders[i].number, ex.Message);
                                else if (ex.pages == Page.TypeEnum.Home)
                                    Log.Warning("Errors => {@order} - {@ex}", orders[i].number, ex.Message);
                                else
                                    Log.Warning("Warnings => {@order} - {@ex}", orders[i].number, ex.Message);

                                continue;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"{orders[i].number} - {ex.Message}");
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
                _changingOrderPage.ConfirmOrder(driver, wait);

                for (int i = 0; i < order.itens.Count(); i++)
                {
                    if (order.itens[i].qtde_volo == 0)
                        _changingOrderPage.RemoveItemFromList(order.itens[i].cod_product_volo, driver, wait);
                    else if (order.itens[i].qtde_volo != order.itens[i].qtde_microvix)
                        _changingOrderPage.ChangeQtdeItemFromList(order.itens[i].cod_product_volo, order.itens[i].qtde_volo, driver, wait);
                }

                _changingOrderPage.ProceedOrder(driver, wait);
                return _changingOrderPage.AproveOrder(driver, wait);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
