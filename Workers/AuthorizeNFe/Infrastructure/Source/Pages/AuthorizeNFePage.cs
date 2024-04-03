using BloomersWorkers.AuthorizeNFe.Domain.Entities;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkers.AuthorizeNFe.Infrastructure.Source.Pages
{
    public class AuthorizeNFePage : IAuthorizeNFePage
    {
        public bool GetResults(string parentWindowHandle, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var searchresponse = ExtensionsMethods.GetElementExistsByXpath("//*[@id=\"FormPaginacao\"]/div[3]", _wait, Page.TypeEnum.AuthorizeNFe, "GetResults");

                while (searchresponse.Text != "Sua pesquisa não encontrou nenhum documento correspondente.")
                {
                    int index_nota = 0, index_situacao = 0, index_opcoes = 0;
                    var table = ExtensionsMethods.GetElementsPresentsByXpath("//*[@id=\"FormPaginacao\"]/div[3]/table/tbody", _wait, Page.TypeEnum.AuthorizeNFe, "GetResults");
                    var rows = ExtensionsMethods.GetElementsExistsByTagName("tr", Page.TypeEnum.AuthorizeNFe, "GetResults", table.First());
                    var ths = ExtensionsMethods.GetElementsExistsByTagName("th", Page.TypeEnum.AuthorizeNFe, "GetResults", rows.First());
                    
                    //read table header
                    for (int col = 0; col < ths.Count(); col++)
                    {
                        if (ths[col].Text == "No.")
                            index_nota = col;
                        if (ths[col].Text == "Situação")
                            index_situacao = col;
                        if (ths[col].Text == "Opções")
                            index_opcoes = col;
                    }

                    //read table lê as linhas da tabela (ignora header)
                    for (int row = 1; row < rows.Count(); row++)
                    {
                        var tds = ExtensionsMethods.GetElementsExistsByTagName("td", Page.TypeEnum.AuthorizeNFe, "GetResults", rows[row]);

                        //if (tds[index_situacao].Text == "5 - Pendente de Autorização") //5 - Pendente de Autorização
                        //{
                        //    tds[index_opcoes].FindElement(By.Id($"flyout_{row}")).Click();

                        //    IWebElement btnConsultarProcessamentoNFe = ExtensionsMethods.ElementToBeClickable(By.XPath("/html/body/div[4]/div/ul/li[5]/a"), _driver);
                        //    btnConsultarProcessamentoNFe.Click();
                        //    Thread.Sleep(30 * 1000);

                        //    IWebElement mensagem = ExtensionsMethods.GetElement(By.Id("mensagem"), _driver);
                        //    var paragrafos = mensagem.FindElements(By.TagName("p"));

                        //    if (paragrafos[0].Text == "(100) Autorizado o uso da NF-e.")
                        //    {
                        //        IWebElement btnOk = ExtensionsMethods.GetElement(By.XPath("/html/body/div[6]/div[11]/div/button"), _driver);
                        //        btnOk.Click();
                        //    }

                        //    if (searchresponse.Text == "Sua pesquisa não encontrou nenhum documento correspondente.")
                        //    {
                        //        _driver.Close();
                        //        _driver.SwitchTo().Window(parentWindowHandle);
                        //        return true;
                        //    }
                        //    else
                        //        break;
                        //}

                        if (tds[index_situacao].Text == "6 - Pendente de Envio") //6 - Pendente de Envio
                        {
                            tds[index_opcoes].FindElement(By.Id($"flyout_{row}")).Click();
                            IWebElement btnEnviarArquivoIndividual = ExtensionsMethods.ElementToBeClickable(By.XPath("/html/body/div[4]/div/ul/li[6]/a"), _driver);
                            btnEnviarArquivoIndividual.Click();
                            Thread.Sleep(30 * 1000);

                            IWebElement mensagem = ExtensionsMethods.GetElement(By.Id("envioIndividual"), _driver);
                            var paragrafos = mensagem.FindElements(By.TagName("p"));

                            if (paragrafos[0].Text == "(103) Lote Recebido com sucesso.")
                            {
                                IWebElement btnOk = ExtensionsMethods.GetElement(By.XPath("/html/body/div[6]/div[11]/div/button"), _driver);
                                btnOk.Click();

                                _driver.Close();
                                _driver.SwitchTo().Window(parentWindowHandle);
                                return true;
                            }
                        }

                        if (tds[index_situacao].Text == "7 - Pendente de Geração") //6 - Pendente de Geração
                        {
                            tds[index_opcoes].FindElement(By.Id($"flyout_{row}")).Click();
                            IWebElement btnGerarArquivoNFe = ExtensionsMethods.ElementToBeClickable(By.XPath("/html/body/div[4]/div/ul/li[8]/a"), _driver);

                            Actions action = new Actions(_driver);
                            action.MoveToElement(btnGerarArquivoNFe).Perform();

                            IWebElement btnNormal = ExtensionsMethods.ElementToBeClickable(By.XPath("/html/body/div[4]/div/ul/li[8]/ul/li[1]/a"), _driver);
                            btnNormal.Click();
                            Thread.Sleep(30 * 1000);

                            IWebElement mensagem = ExtensionsMethods.GetElement(By.Id("mensagem"), _driver);
                            var paragrafos = mensagem.FindElements(By.TagName("p"));

                            if (paragrafos[0].Text == "(M95) Geração da NF-e realizada com sucesso.")
                            {
                                IWebElement btnOk = ExtensionsMethods.GetElement(By.XPath("/html/body/div[6]/div[11]/div/button"), _driver);
                                btnOk.Click();

                                _driver.Close();
                                _driver.SwitchTo().Window(parentWindowHandle);
                                return true;
                            }
                        }

                        if (tds[index_situacao].Text == "9 - Rejeitada") //9 - Rejeitada
                        {
                            tds[index_opcoes].FindElement(By.Id($"flyout_{row}")).Click();
                            IWebElement btnExcluirNFe = ExtensionsMethods.ElementToBeClickable(By.XPath("/html/body/div[4]/div/ul/li[9]/a"), _driver);
                            btnExcluirNFe.Click();

                            Thread.Sleep(30 * 1000);

                            IWebElement mensagem = ExtensionsMethods.GetElement(By.Id("mensagem"), _driver);
                            var paragrafos = mensagem.FindElements(By.TagName("p"));

                            if (paragrafos[0].Text == "(M618) Sucesso ao excluir a NF-e .")
                            {
                                IWebElement btnOk = ExtensionsMethods.GetElement(By.XPath("/html/body/div[6]/div[11]/div/button"), _driver);
                                btnOk.Click();

                                _driver.Close();
                                _driver.SwitchTo().Window(parentWindowHandle);
                                return true;
                            }
                        }
                    }
                    Thread.Sleep(5 * 1000);
                    searchresponse = ExtensionsMethods.GetElement(By.XPath("//*[@id=\"FormPaginacao\"]/div[3]"), _driver);
                }
                _driver.Close();
                _driver.SwitchTo().Window(parentWindowHandle);
                return false;
            }
            catch
            {
                throw;
            }
        }

        public string NavigateToNFeTab(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                Thread.Sleep(2 * 1000);

                var windowHandle = string.Empty;
                var parentWindowHandle = _driver.CurrentWindowHandle;
                var lstWindows = _driver.WindowHandles.ToList();

                foreach (var window in lstWindows)
                {
                    if (window != parentWindowHandle)
                    {
                        windowHandle = window;
                        _driver.SwitchTo().Window(window);
                        _driver.Manage().Window.Maximize();
                    }
                    else
                        continue;
                }

                Thread.Sleep(1 * 1000);

                var flagModalCertificated = ExtensionsMethods.ChecksIfElementExistInAnotherWindowById("exibeMsgCertificado", _wait, Page.TypeEnum.AuthorizeNFe, "NavigateToNFeTab", windowHandle, _driver);
                var buttonCloseModal = ExtensionsMethods.GetElementIfExistsByXpath("/html/body/div[3]/div[11]/div/button", _wait, Page.TypeEnum.AuthorizeNFe, "NavigateToNFeTab");
                var buttonNFeInsideWindow = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"nfeNFe\"]/a", _wait, Page.TypeEnum.AuthorizeNFe, "NavigateToNFeTab");

                if (flagModalCertificated)
                    ExtensionsMethods.ClickInElement(buttonCloseModal);

                ExtensionsMethods.ClickInElement(buttonNFeInsideWindow);
                return parentWindowHandle;
            }
            catch
            {
                throw;
            }
        }

        public void SetFilters(Order order, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var inputInicialPeriod = ExtensionsMethods.GetElementToBeClickableById("periodoInicial", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");
                var inputFinalPeriod = ExtensionsMethods.GetElementToBeClickableById("periodoFinal", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");
                var inputNumberNFe = ExtensionsMethods.GetElementToBeClickableById("numero", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");
                var checkBoxIsAuthorized = ExtensionsMethods.GetElementToBeClickableById("isAutorizada", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");
                var checkBoxIsDenied = ExtensionsMethods.GetElementToBeClickableById("isDenegada", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");
                var checkBoxIsCanceled = ExtensionsMethods.GetElementToBeClickableById("isCancelada", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");
                var checkBoxIsUnused = ExtensionsMethods.GetElementToBeClickableById("isInutilizada", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");
                var checkBoxIsExcluded = ExtensionsMethods.GetElementToBeClickableById("isExcluida", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");
                var buttonAction = ExtensionsMethods.GetElementToBeClickableById("btnAcao", _wait, Page.TypeEnum.AuthorizeNFe, "SetFilters");

                ExtensionsMethods.ClearInputElementValue(inputInicialPeriod);
                ExtensionsMethods.ClearInputElementValue(inputFinalPeriod);

                ExtensionsMethods.ClickInElement(inputInicialPeriod);
                ExtensionsMethods.SendKeysToElement(inputInicialPeriod, order.invoice.date_emission_nf.ToShortDateString());

                ExtensionsMethods.ClickInElement(inputFinalPeriod);
                ExtensionsMethods.SendKeysToElement(inputFinalPeriod, order.invoice.date_emission_nf.ToShortDateString());

                ExtensionsMethods.SendKeysToElement(inputNumberNFe, order.invoice.number_nf);

                //ExtensionsMethods.ClickInElement(checkBoxIsAuthorized);
                //ExtensionsMethods.ClickInElement(checkBoxIsDenied);
                //ExtensionsMethods.ClickInElement(checkBoxIsCanceled);
                //ExtensionsMethods.ClickInElement(checkBoxIsUnused);
                //ExtensionsMethods.ClickInElement(checkBoxIsExcluded);
                ExtensionsMethods.ClickInElement(buttonAction);
            }
            catch
            {
                throw;
            }
        }

        private void PendingAuthorization(System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>? tds, IWebElement searchresponse, IWebDriver _driver, int index_situacao, int index_opcoes, int row, string parentWindowHandle)
        {
            try
            {
                if (tds[index_situacao].Text == "5 - Pendente de Autorização") //5 - Pendente de Autorização
                {
                    tds[index_opcoes].FindElement(By.Id($"flyout_{row}")).Click();

                    IWebElement btnConsultarProcessamentoNFe = ExtensionsMethods.ElementToBeClickable(By.XPath("/html/body/div[4]/div/ul/li[5]/a"), _driver);
                    btnConsultarProcessamentoNFe.Click();
                    Thread.Sleep(30 * 1000);

                    IWebElement mensagem = ExtensionsMethods.GetElement(By.Id("mensagem"), _driver);
                    var paragrafos = mensagem.FindElements(By.TagName("p"));

                    if (paragrafos[0].Text == "(100) Autorizado o uso da NF-e.")
                    {
                        IWebElement btnOk = ExtensionsMethods.GetElement(By.XPath("/html/body/div[6]/div[11]/div/button"), _driver);
                        btnOk.Click();
                    }

                    if (searchresponse.Text == "Sua pesquisa não encontrou nenhum documento correspondente.")
                    {
                        _driver.Close();
                        _driver.SwitchTo().Window(parentWindowHandle);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void PendingShipping()
        {

        }

        private void GenerationPending()
        {

        }

        private void Rejected()
        {

        }
    }
}
