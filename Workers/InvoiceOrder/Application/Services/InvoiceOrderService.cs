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
        private readonly IDriver _chromeDriver;
        private readonly IConfiguration _configuration;
        private readonly IInvoiceOrderRepository _invoiceOrderRepository;

        public InvoiceOrderService(ILoginPage loginPage, IHomePage homePage, IVDPage vdPage, IB2CPage b2cPage, IDriver chromeDriver, IConfiguration configuration, IInvoiceOrderRepository invoiceOrderRepository) =>
            (_loginPage, _homePage, _vdPage, _b2cPage, _chromeDriver, _configuration, _invoiceOrderRepository) = (loginPage, homePage, vdPage, b2cPage, chromeDriver, configuration, invoiceOrderRepository);

        public async Task InvoiceOrder()
        {
            try
            {
                var workerName = $"{_configuration.GetSection("ConfigureService").GetSection("InvoiceOrder").GetSection("BotName").Value} {_configuration.GetSection("ConfigureService").GetSection("InvoiceOrder").GetSection("FinalIdControle").Value}";
                var orders = await _invoiceOrderRepository.GetOrdersFromIT4(workerName);
                var user = await _invoiceOrderRepository.GetMicrovixUser(workerName);

                if (orders.Count() > 0)
                {
                    Log.Information($"Obtidos: {orders.Count()} pedidos para serem faturados");
                    var driver = _chromeDriver.GetChromeDriverInstance();
                    var wait = _chromeDriver.GetWebDriverWaitInstance(driver);

                    for (int i = 0; i < orders.Count(); i++)
                    {
                        try
                        {
                            Log.Information($"Iniciando o faturamento do pedido {orders[i].number}");
                            if (i == 0)
                            {
                                Log.Information("Realizando o login no ERP");
                                _loginPage.InsertLoginAndPassword(user, wait);

                                Log.Information("Selecionando empresa");
                                _loginPage.SelectCompany(orders[i].company.doc_company, driver, wait);
                            }
                            else
                            {
                                Log.Information("Selecionando empresa");
                                _loginPage.SelectCompanyFromTopBar(orders[i].company.doc_company, wait);
                            }

                            if (InvoiceOrder(orders[i], driver, wait))
                            {
                                Log.Information($"Pedido: {orders[i].number}, faturado com sucesso");
                                await _invoiceOrderRepository.UpdateInvoiceAttemptIT4(orders[i].number, orders[i].invoice_attempts + 1);
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

                            await _invoiceOrderRepository.UpdateInvoiceAttemptIT4(orders[i].number, orders[i].invoice_attempts + 1);

                            continue;
                        }

                        catch (Exception ex)
                        {
                            Log.Error("Errors => {@ex}", ex.Message);

                            await _invoiceOrderRepository.UpdateInvoiceAttemptIT4(orders[i].number, orders[i].invoice_attempts + 1);

                            continue;
                        }

                        //finaly
                        //_chromeDriver.Dispose();
                    }

                    _chromeDriver.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Errors => {@ex}", ex.Message);
            }
        }

        public async Task InvoiceOrder(string workerName)
        {
            try
            {
                var orders = await _invoiceOrderRepository.GetOrdersFromIT4(workerName);
                var user = await _invoiceOrderRepository.GetMicrovixUser(workerName);

                if (orders.Count() > 0)
                {
                    Log.Information($"Obtidos: {orders.Count()} pedidos para serem faturados");
                    var driver = _chromeDriver.GetChromeDriverInstance();
                    var wait = _chromeDriver.GetWebDriverWaitInstance(driver);

                    for (int i = 0; i < orders.Count(); i++)
                    {
                        try
                        {
                            Log.Information($"Iniciando o faturamento do pedido {orders[i].number}");
                            if (i == 0)
                            {
                                Log.Information("Realizando o login no ERP");
                                _loginPage.InsertLoginAndPassword(user, wait);

                                Log.Information("Selecionando empresa");
                                _loginPage.SelectCompany(orders[i].company.doc_company, driver, wait);
                            }
                            else
                            {
                                Log.Information("Selecionando empresa");
                                _loginPage.SelectCompanyFromTopBar(orders[i].company.doc_company, wait);
                            }

                            if (InvoiceOrder(orders[i], driver, wait))
                            {
                                Log.Information($"Pedido: {orders[i].number}, faturado com sucesso");
                                await _invoiceOrderRepository.UpdateInvoiceAttemptIT4(orders[i].number, orders[i].invoice_attempts + 1);
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

                            await _invoiceOrderRepository.UpdateInvoiceAttemptIT4(orders[i].number, orders[i].invoice_attempts + 1);

                            continue;
                        }

                        catch (Exception ex)
                        {
                            Log.Error("Errors => {@ex}", ex.Message);

                            await _invoiceOrderRepository.UpdateInvoiceAttemptIT4(orders[i].number, orders[i].invoice_attempts + 1);

                            continue;
                        }
                    }

                    _chromeDriver.Dispose();
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
                Log.Information("Fechando modal de notas pendentes");
                _homePage.ClosePendingInvoicesModal(driver, wait);
                Log.Information("Abrindo o menu lateral");
                _homePage.OpenSideMenu(driver, wait);

                #region VENDA DIRETA
                if (order.number.Contains("OA-VD") || order.number.Contains("OA-LJ") || order.number.Contains("MI-VD") || order.number.Contains("MI-LJ"))
                {
                    Log.Information("Navegando para tela de faturamento");
                    _homePage.NavigateToVDScreen(driver, wait);
                    Log.Information("Selecionando o pedido");
                    _vdPage.SelectOrder(order.number, driver, wait);
                    Log.Information("Incluindo dados do pedido");
                    _vdPage.SetOrderData(order.company.doc_company, driver, wait);
                    Log.Information("Incluindo dados da transportadora");
                    _vdPage.SetShippimentData(order.company.doc_company, order.shippingCompany.cod_shippingCompany, order.volumes, driver, wait);
                    Log.Information("Incluindo dados do centro de custo");
                    _vdPage.SetCostCenter(order.company.doc_company, driver, wait);

                    return _vdPage.WaitChaveNFe(driver, wait);
                }
                #endregion

                #region B2C
                else
                {
                    Log.Information("Navegando para tela de pedidos");
                    _homePage.NavigateToB2CScreen(driver, wait);
                    Log.Information("Selecionando o pedido");
                    _b2cPage.SelectOrder(order.number, driver, wait);
                    Log.Information("Incluindo dados do pedido");
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
