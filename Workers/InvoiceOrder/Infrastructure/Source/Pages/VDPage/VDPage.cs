using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages
{
    public class VDPage : IVDPage
    {
        public void SelectOrder(string nr_pedido, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                Thread.Sleep(5 * 1000);

                ExtensionsMethods.ChangeToFrameWhenItsAvaiable("main", _wait, Page.TypeEnum.InvoiceOrder, "SelectOrder");

                var order = $"{nr_pedido.Replace("OA-VD", "").Replace("OA-LJ", "").Replace("MX-VD", "").Replace("MI-VD", "").Replace("MI-LJ", "")}";
                var buttonProceed = ExtensionsMethods.GetElementToBeClickableById("B2", _wait, Page.TypeEnum.InvoiceOrder, "SelectOrder");
                var inputDocument = ExtensionsMethods.GetElementToBeClickableById("documento", _wait, Page.TypeEnum.InvoiceOrder, "SelectOrder");

                if (inputDocument.Displayed && inputDocument.Enabled)
                {
                    var comboboxOrder = new SelectElement(inputDocument);
                    comboboxOrder.SelectByValue($"{order}");
                    ExtensionsMethods.ClickInElement(buttonProceed);
                }
            }
            catch
            {
                throw;
            }
        }

        public void SetOrderData(string cnpj, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var serie = string.Empty;
                if (cnpj == "42538267000268")
                    serie = "4 (Num.Autom.)";
                else if (cnpj == "38367316000199")
                    serie = "3 (Num.Autom.)";
                else
                    serie = "1 (Num.Autom.)";

                SelectElement comboboxSerie = new SelectElement(_wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("serie"))));
                comboboxSerie.SelectByText(serie);

                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_datasaida"))).SendKeys($"{DateTime.Today.ToString("dd/MM/yyyy")}");

                if (DateTime.Now.Hour < 23)
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_horasaida"))).SendKeys($"{DateTime.Now.AddHours(1).ToString("HH:mm")}");
                else
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_horasaida"))).SendKeys($"{DateTime.Now.ToString("HH:mm")}");

                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("B1"))).Click();
            }
            catch
            {
                throw;
            }
        }

        public void SetShippimentData(string cnpj, string cod_transportadora, string volumes, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                Thread.Sleep(5 * 1000);
                IWebElement frete = _wait.Until(ExpectedConditions.ElementExists(By.Id("frete")));
                IWebElement tipoFreteEmitente = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("tipo_frete1")));
                IWebElement tipoFreteDestinatario = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("tipo_frete2")));
                IWebElement tipoFreteSemCobranca = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("tipo_frete6")));
                IWebElement tipoFreteRedespachoEmitente = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("tipo_frete_redespacho1")));
                IWebElement tipoFreteRedespachoDestinatario = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("tipo_frete_redespacho2")));
                IWebElement tipoFreteRedespachoSemCobranca = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("tipo_frete_redespacho6")));
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("quantidade"))).SendKeys(volumes);

                if (cnpj == "42538267000268")
                {
                    ExtensionsMethods.ClickInElement(tipoFreteEmitente);
                    ExtensionsMethods.ClickInElement(tipoFreteRedespachoEmitente);

                    //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='1'", tipoFrete);
                    //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='1'", tipoFreteRedespacho);
                    ((IJavaScriptExecutor)_driver).ExecuteScript($"arguments[0].value='{cod_transportadora}'", ExtensionsMethods.GetElement(By.Id("transportador"), _driver));

                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("especie"))).SendKeys("CX");
                }
                else
                {
                    if (frete.GetAttribute("value") == "0,00")
                    {
                        ExtensionsMethods.ClickInElement(tipoFreteEmitente);
                        ExtensionsMethods.ClickInElement(tipoFreteRedespachoEmitente);
                        //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='1'", tipoFrete);
                        //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='1'", tipoFreteRedespacho);
                    }
                    else
                    {
                        ExtensionsMethods.ClickInElement(tipoFreteDestinatario);
                        ExtensionsMethods.ClickInElement(tipoFreteRedespachoDestinatario);
                        //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='2'", tipoFrete);
                        //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='2'", tipoFreteRedespacho);
                    }
                    ((IJavaScriptExecutor)_driver).ExecuteScript($"arguments[0].value='{cod_transportadora}'", ExtensionsMethods.GetElement(By.Id("transportador"), _driver));

                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("especie"))).SendKeys("PACOTE");
                }

                Thread.Sleep(3 * 1000);
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btConfirmOff"))).Click();
                Thread.Sleep(3 * 1000);

                if (_wait.Until(ExpectedConditions.ElementExists(By.Id("dialogAlerta"))).Displayed)
                {
                    var message = _wait.Until(ExpectedConditions.ElementExists(By.Id("mensagem"))).Text;
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("ui-button-text-only"))).Click();
                    throw new Exception($@" - VDPage (SetShippimentData) - Erro ao selecionar dados da transportadora do pedido - {message}");
                }

                Thread.Sleep(4 * 1000);
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("B1"))).Click();
            }
            catch
            {
                throw;
            }
        }

        public void SetCostCenter(string cnpj, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                string locator = string.Empty;

                if (ExtensionsMethods.ElementExist(By.Id("centro_custo0"), _driver))
                {
                    if (_wait.Until(ExpectedConditions.ElementExists(By.Id("centro_custo0"))).Enabled && _wait.Until(ExpectedConditions.ElementExists(By.Id("centro_custo0"))).Displayed)
                    {
                        if (cnpj == "42538267000268")
                            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='17'", _wait.Until(ExpectedConditions.ElementExists(By.Id("centro_custo0"))));
                        locator = "confirmar";
                    }
                }

                else if (ExtensionsMethods.ElementExist(By.Id("centro_custo"), _driver))
                {
                    if (_wait.Until(ExpectedConditions.ElementExists(By.Id("centro_custo"))).Displayed && _wait.Until(ExpectedConditions.ElementExists(By.Id("centro_custo"))).Enabled)
                    {
                        if (cnpj == "42538267000268")
                            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='17'", _wait.Until(ExpectedConditions.ElementExists(By.Id("centro_custo"))));
                        locator = "B1";
                    }
                }

                if (_wait.Until(ExpectedConditions.ElementExists(By.Name($"{locator}"))).Displayed && _wait.Until(ExpectedConditions.ElementExists(By.Name($"{locator}"))).Enabled)
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.Name($"{locator}"))).Click();
            }
            catch
            {
                throw;
            }
        }

        public bool WaitChaveNFe(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                IWebElement frameObject = _wait.Until(ExpectedConditions.ElementExists(By.Id("object-nfe")));
                _driver.SwitchTo().Frame(frameObject);

                if (ExtensionsMethods.ElementExist(By.Id("div-sucesso"), _driver))
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
