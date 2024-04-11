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

                        if (PendingAuthorization(tds[index_opcoes], tds[index_situacao].Text, _wait, searchresponse.Text, _driver, row, parentWindowHandle))
                            return true;

                        else if (PendingShipping(tds[index_opcoes], tds[index_situacao].Text, _wait, _driver, row, parentWindowHandle))
                            return true;

                        else if (GenerationPending(tds[index_opcoes], tds[index_situacao].Text, _wait, _driver, row, parentWindowHandle))
                            return true;

                        else if (Rejected(tds[index_opcoes], tds[index_situacao].Text, _wait, _driver, row, parentWindowHandle))
                            return true;
                        
                        else
                            break;
                    }
                    Thread.Sleep(4 * 1000);
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

        private bool PendingAuthorization(IWebElement tds, string tdsText, WebDriverWait _wait, string searchresponseText, IWebDriver _driver, int row, string parentWindowHandle)
        {
            try
            {
                if (tdsText == "5 - Pendente de Autorização")
                {
                    var buttonPendingAuthorization = ExtensionsMethods.GetElementInsideAnotherElementById($"flyout_{row}", Page.TypeEnum.AuthorizeNFe, "PendingAuthorization", tds);
                    var buttonConsultProcessingNFe = ExtensionsMethods.GetElementToBeClickableByXpath("/html/body/div[4]/div/ul/li[5]/a", _wait, Page.TypeEnum.AuthorizeNFe, "PendingAuthorization");

                    ExtensionsMethods.ClickInElement(buttonPendingAuthorization);
                    ExtensionsMethods.ClickInElement(buttonConsultProcessingNFe);
                    
                    Thread.Sleep(30 * 1000);

                    var labelMessage = ExtensionsMethods.GetElementExistsById("mensagem", _wait, Page.TypeEnum.AuthorizeNFe, "PendingAuthorization");
                    var paragraphs = ExtensionsMethods.GetElementsExistsByTagName("p", Page.TypeEnum.AuthorizeNFe, "PendingAuthorization", labelMessage);

                    if (paragraphs[0].Text == "(100) Autorizado o uso da NF-e.")
                    {
                        var buttonOk = ExtensionsMethods.GetElementExistsByXpath("/html/body/div[6]/div[11]/div/button", _wait, Page.TypeEnum.AuthorizeNFe, "PendingAuthorization");
                        ExtensionsMethods.ClickInElement(buttonOk);
                    }

                    if (searchresponseText == "Sua pesquisa não encontrou nenhum documento correspondente.")
                    {
                        _driver.Close();
                        _driver.SwitchTo().Window(parentWindowHandle);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        private bool PendingShipping(IWebElement tds, string tdsText, WebDriverWait _wait, IWebDriver _driver, int row, string parentWindowHandle)
        {
            if (tdsText == "6 - Pendente de Envio")
            {
                var buttonPendingShipping = ExtensionsMethods.GetElementInsideAnotherElementById($"flyout_{row}", Page.TypeEnum.AuthorizeNFe, "PendingShipping", tds);
                var sendIndividualFile = ExtensionsMethods.GetElementToBeClickableByXpath("/html/body/div[4]/div/ul/li[6]/a", _wait, Page.TypeEnum.AuthorizeNFe, "PendingShipping");

                ExtensionsMethods.ClickInElement(buttonPendingShipping);
                ExtensionsMethods.ClickInElement(sendIndividualFile);
                
                Thread.Sleep(30 * 1000);

                var labelMessage = ExtensionsMethods.GetElementExistsById("envioIndividual", _wait, Page.TypeEnum.AuthorizeNFe, "PendingShipping");
                var paragraphs = ExtensionsMethods.GetElementsExistsByTagName("p", Page.TypeEnum.AuthorizeNFe, "PendingShipping", labelMessage);

                if (paragraphs[0].Text == "(103) Lote Recebido com sucesso.")
                {
                    var buttonOk = ExtensionsMethods.GetElementExistsByXpath("/html/body/div[6]/div[11]/div/button", _wait, Page.TypeEnum.AuthorizeNFe, "PendingShipping");
                    ExtensionsMethods.ClickInElement(buttonOk);

                    _driver.Close();
                    _driver.SwitchTo().Window(parentWindowHandle);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private bool GenerationPending(IWebElement tds, string tdsText, WebDriverWait _wait, IWebDriver _driver, int row, string parentWindowHandle)
        {
            if (tdsText == "7 - Pendente de Geração")
            {
                var buttonGenerationPending = ExtensionsMethods.GetElementInsideAnotherElementById($"flyout_{row}", Page.TypeEnum.AuthorizeNFe, "GenerationPending", tds);
                ExtensionsMethods.ClickInElement(buttonGenerationPending);

                var buttonGenerateFileNFe = ExtensionsMethods.GetElementToBeClickableByXpath("/html/body/div[4]/div/ul/li[8]/a", _wait, Page.TypeEnum.AuthorizeNFe, "GenerationPending");
                Actions action = new Actions(_driver);
                action.MoveToElement(buttonGenerateFileNFe).Perform();

                var buttonNormal = ExtensionsMethods.GetElementToBeClickableByXpath("/html/body/div[4]/div/ul/li[8]/ul/li[1]/a", _wait, Page.TypeEnum.AuthorizeNFe, "GenerationPending");
                ExtensionsMethods.ClickInElement(buttonNormal);

                Thread.Sleep(30 * 1000);

                var labelMessage = ExtensionsMethods.GetElementExistsById("mensagem", _wait, Page.TypeEnum.AuthorizeNFe, "GenerationPending");
                var paragraphs = ExtensionsMethods.GetElementsExistsByTagName("p", Page.TypeEnum.AuthorizeNFe, "GenerationPending", labelMessage);

                if (paragraphs[0].Text == "(M95) Geração da NF-e realizada com sucesso.")
                {
                    var buttonOk = ExtensionsMethods.GetElementExistsByXpath("/html/body/div[6]/div[11]/div/button", _wait, Page.TypeEnum.AuthorizeNFe, "PendingShipping");
                    ExtensionsMethods.ClickInElement(buttonOk);

                    _driver.Close();
                    _driver.SwitchTo().Window(parentWindowHandle);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private bool Rejected(IWebElement tds, string tdsText, WebDriverWait _wait, IWebDriver _driver, int row, string parentWindowHandle)
        {
            if (tdsText == "9 - Rejeitada")
            {
                var buttonRejected = ExtensionsMethods.GetElementInsideAnotherElementById($"flyout_{row}", Page.TypeEnum.AuthorizeNFe, "Rejected", tds);
                ExtensionsMethods.ClickInElement(buttonRejected);

                var buttonDeleteNFe = ExtensionsMethods.GetElementToBeClickableByXpath("/html/body/div[4]/div/ul/li[9]/a", _wait, Page.TypeEnum.AuthorizeNFe, "Rejected");
                ExtensionsMethods.ClickInElement(buttonDeleteNFe);

                Thread.Sleep(30 * 1000);

                var labelMessage = ExtensionsMethods.GetElementExistsById("mensagem", _wait, Page.TypeEnum.AuthorizeNFe, "Rejected");
                var paragraphs = ExtensionsMethods.GetElementsExistsByTagName("p", Page.TypeEnum.AuthorizeNFe, "Rejected", labelMessage);

                if (paragraphs[0].Text == "(M618) Sucesso ao excluir a NF-e .")
                {
                    var buttonOk = ExtensionsMethods.GetElementExistsByXpath("/html/body/div[6]/div[11]/div/button", _wait, Page.TypeEnum.AuthorizeNFe, "PendingShipping");
                    ExtensionsMethods.ClickInElement(buttonOk);

                    _driver.Close();
                    _driver.SwitchTo().Window(parentWindowHandle);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
