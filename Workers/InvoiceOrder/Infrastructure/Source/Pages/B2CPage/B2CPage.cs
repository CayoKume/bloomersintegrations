using BloomersWorkersCore.Domain.CustomExceptions;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages
{
    public class B2CPage : IB2CPage
    {
        public void SelectOrder(string nr_pedido, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                Thread.Sleep(4 * 1000);

                _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("main")));
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"div-main\"]/div[1]/div[1]/div[2]/div/input"))).SendKeys($"{nr_pedido}");

                Thread.Sleep(2 * 1000);

                if (!ExtensionsMethods.ElementIsVisible(By.ClassName("alert-info"), _driver))
                    _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id=\"div-main\"]/div[1]/div[3]/div/div/div/div/div[1]/div[1]"))).FirstOrDefault().FindElement(By.XPath("//*[@id=\"div-main\"]/div[1]/div[3]/div/div/div/div/div[1]/div[1]/label")).Click();
                else
                    throw new Exception(@$"pedido nao encontrado");

                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnFaturarRodape"))).Click();
            }
            catch (Exception ex) when (ex.Message.Contains("pedido nao encontrado"))
            {
                throw new CustomNoSuchElementException(
                    @$"O pedido não está disponível para faturamento, verifique se ele já não foi faturado, ou se não está cancelado",
                    ex.Message,
                    Page.TypeEnum.B2C
                );
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$"B2CPage (SelectOrder) - O bot nao foi capaz de encontrar o elemento para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@"B2CPage (SelectOrder) - Erro ao selecionar pedido para ser faturado - {ex.Message}");
            }
        }

        public bool SetOrderData(string cod_transportadora, string cnpj, string nr_pedido, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var serie = string.Empty;
                if (cnpj == "38367316000199")
                    serie = "3";
                else
                    serie = "4";

                var transportadora = string.Empty;
                if (cod_transportadora.Contains("18035"))
                    transportadora = "TRES FLASHS - COLETAS E ENTREGAS RAPIDAS LTDA (CNPJ: 09559012000206)";
                else if (cod_transportadora.Contains("7601"))
                    transportadora = "TEX COURIER S.A (CNPJ: 73939449000193)";
                else if (cod_transportadora.Contains("3535"))
                    transportadora = "AWR EXPRESS TRANSPORTES LTDA (CNPJ: 08298621000105)";
                else if (cod_transportadora.Contains("1210"))
                    transportadora = "AGENCIA DE POSTAGEM FARIA LIMA LTDA (CNPJ: 01902487000160)";
                else if (cod_transportadora.Contains("3757"))
                    transportadora = "RAFAELLA FEITOSA / AMERICO VANELLI NETO 36151247825 (CNPJ: 45396924000197)";
                else if (cod_transportadora.Contains("3932"))
                    transportadora = "MARCOS AURELIO DO NASCIMENTO 33384667816 (CNPJ: 20636564000122)";
                else if (cod_transportadora.Contains("40164"))
                    transportadora = "MANDAE SERVICOS DE CONSULTORIA EM LOGISTICA S/A (CNPJ: 19782476000150)";

                Thread.Sleep(2 * 1000);

                var modalFaturarVendas = _wait.Until(ExpectedConditions.ElementExists(By.Id("modalFaturarVendas")));
                var comboboxesFaturamento = _wait.Until(ExpectedConditions.ElementExists(By.Id("modalFaturarVendas"))).FindElements(By.ClassName("col-sm-12"));
                foreach (var comboboxFaturamento in comboboxesFaturamento)
                {
                    if (comboboxFaturamento.FindElement(By.TagName("label")).Text == "Série")
                    {
                        IWebElement comboboxSerie = ExtensionsMethods.ElementToBeClickable(comboboxFaturamento.FindElement(By.ClassName("multiselect__tags")), _driver);
                        if (comboboxSerie.Text == "Selecione")
                        {
                            comboboxSerie.Click();
                            IWebElement inputSerie = ExtensionsMethods.ElementToBeClickable(comboboxFaturamento.FindElement(By.Id("serieSelecionadaFaturamento")), _driver);
                            Thread.Sleep(500);
                            inputSerie.SendKeys(serie);
                            inputSerie.SendKeys(Keys.Enter);
                        }
                    }
                    else if (comboboxFaturamento.FindElement(By.TagName("label")).Text == "Transportador")
                    {
                        IWebElement comboboxTransporte = ExtensionsMethods.ElementToBeClickable(comboboxFaturamento.FindElement(By.ClassName("multiselect__tags")), _driver);
                        if (comboboxTransporte.Text == "Selecione")
                        {
                            comboboxTransporte.Click();
                            IWebElement inputTransportador = ExtensionsMethods.ElementToBeClickable(comboboxFaturamento.FindElement(By.Id("transportadorSelecionadoFaturamento")), _driver);
                            inputTransportador.SendKeys(transportadora);
                            inputTransportador.SendKeys(Keys.Enter);
                            break;
                        }
                        else
                            break;
                    }
                }

                IWebElement btnFaturarB2C = ExtensionsMethods.ElementToBeClickable(By.Id("btnConfirmarFaturamento"), _driver);
                btnFaturarB2C.Click();
                Thread.Sleep(15 * 1000);

                if (ExtensionsMethods.ElementExist(By.ClassName("icone-autorizada-status-nfe-venda"), _driver))
                    return true;

                else if (ExtensionsMethods.ElementExist(By.ClassName("icone-rejeicao-status-nfe-venda"), _driver))
                    throw new Exception($@" - B2CPage (SetOrderData) - Erro ao faturar o pedido: {nr_pedido}, rejeição da sefaz retornada - {ExtensionsMethods.GetElement(By.ClassName("icone-rejeicao-status-nfe-venda"), _driver).FindElement(By.XPath("..")).Text}");

                else if (ExtensionsMethods.ElementExist(By.Id("swal2-content"), _driver))
                {
                    IWebElement btnFechar = ExtensionsMethods.ElementToBeClickable(By.XPath("/html/body/div[4]/div/div[3]/button[1]"), _driver);
                    btnFechar.Click();
                    throw new Exception($@" - B2CPage (SetOrderData) - Erro ao faturar o pedido: {nr_pedido}, porém houve alguma inconsistência: {ExtensionsMethods.GetElement(By.Id("swal2-content"), _driver).Text} - {ExtensionsMethods.GetElement(By.Id("swal2-content"), _driver).Text}");
                }

                else
                    return false;
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$"B2CPage (SetOrderData) - O bot nao foi capaz de encontrar o elemento para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@"B2CPage (SetOrderData) - Erro ao selecionar pedido para ser faturado - {ex.Message}");
            }
        }
    }
}
