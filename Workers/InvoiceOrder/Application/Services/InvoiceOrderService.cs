using BloomersWorkers.InvoiceOrder.Domain.Entities;
using BloomersWorkers.InvoiceOrder.Infrastructure.Repositorys;
using BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages;
using BloomersWorkersCore.Domain.CustomExceptions;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Infrastructure.Source.Drivers;
using BloomersWorkersCore.Infrastructure.Source.Pages;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;

namespace BloomersWorkers.InvoiceOrder.Application.Services
{
    public class InvoiceOrderService : IInvoiceOrderService
    {
        private readonly ILoginPage _loginPage;
        private readonly IHomePage _homePage;
        private readonly IVDPage _vdPage;
        private readonly IB2CPage _b2cPage;
        private readonly IChromeDriver _chromeDriver;
        private readonly IInvoiceOrderRepository _invoiceOrderRepository;
        private readonly IConfiguration _configuration;

        public InvoiceOrderService(ILoginPage loginPage, IHomePage homePage, IVDPage vdPage, IB2CPage b2cPage, IChromeDriver chromeDriver, IInvoiceOrderRepository invoiceOrderRepository, IConfiguration configuration) =>
            (_loginPage, _homePage, _vdPage, _b2cPage, _chromeDriver, _invoiceOrderRepository, _configuration) = (loginPage, homePage, vdPage, b2cPage, chromeDriver, invoiceOrderRepository, configuration);

        public async Task InvoiceOrder()
        {
            try
            {
                string? botName = $"{_configuration.GetSection("ConfigureService").GetSection("InvoiceOrder").GetSection("BotName").Value} {_configuration.GetSection("ConfigureService").GetSection("InvoiceOrder").GetSection("FinalIdControle").Value}";
                var orders = await _invoiceOrderRepository.GetOrdersFromIT4(botName);

                if (orders.Count() > 0)
                {
                    using (var driver = _chromeDriver.GetChromeDriverInstance())
                    {
                        var wait = _chromeDriver.GetWebDriverWaitInstance(driver);

                        foreach (var order in orders)
                        {
                            try
                            {
                                var user = await _invoiceOrderRepository.GetMicrovixUser(botName);
                                _loginPage.InsertLoginAndPassword(user, wait);
                                _loginPage.SelectCompany(order.company.doc_company, user, driver, wait);

                                if (InvoiceOrder(order, driver, wait))
                                {
                                    Log.Information($"Pedido: {order.number}, faturado com sucesso");
                                    await _invoiceOrderRepository.UpdateInvoiceAttemptIT4(order.number, order.invoice_attempts + 1);
                                }
                            }
                            catch (CustomNoSuchElementException ex)
                            {
                                if (ex.pages == Page.TypeEnum.Login)
                                    Log.Warning("Errors => {@order} - {@ex}", order.number, ex.Message);
                                else if (ex.pages == Page.TypeEnum.Home)
                                    Log.Warning("Errors => {@order} - {@ex}", order.number, ex.Message);
                                else
                                    Log.Warning("Warnings => {@order} - {@ex}", order.number, ex.Message);
                                
                                await _invoiceOrderRepository.UpdateInvoiceAttemptIT4(order.number, order.invoice_attempts + 1);

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

        private bool InvoiceOrder(Order order, IWebDriver driver, WebDriverWait wait)
        {
            try
            {
                #region VENDA DIRETA
                if (order.number.Contains("OA-VD") || order.number.Contains("OA-LJ") || order.number.Contains("MI-VD") || order.number.Contains("MI-LJ"))
                {
                    _homePage.NavigateToVDScreen(driver, wait);
                    _vdPage.SelectOrder(order.number, driver, wait);
                    _vdPage.SetOrderData(order.company.doc_company, driver, wait);
                    _vdPage.SetShippimentData(order.company.doc_company, order.shippingCompany.cod_shippingCompany, order.volumes, driver, wait);
                    _vdPage.SetCostCenter(order.company.doc_company, driver, wait);

                    return _vdPage.WaitChaveNFe(driver, wait);
                }
                #endregion

                #region B2C
                else
                {
                    _homePage.NavigateToB2CScreen(driver, wait);
                    _b2cPage.SelectOrder(order.number, driver, wait);
                    return _b2cPage.SetOrderData(order.shippingCompany.cod_shippingCompany, order.company.doc_company, order.number, driver, wait);
                }
                #endregion
            }
            catch
            {
                throw;
            }
        }
    }
}
