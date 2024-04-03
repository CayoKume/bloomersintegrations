using BloomersWorkers.AuthorizeNFe.Domain.Entities;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkers.AuthorizeNFe.Infrastructure.Source.Pages
{
    public class AuthorizeNFePage : IAuthorizeNFePage
    {
        public bool GetResults(string parentWindowHandle, IWebDriver _driver)
        {
            try
            {
                IWebElement respostaPesquisa = ExtensionsMethods.GetElement(By.XPath("//*[@id=\"FormPaginacao\"]/div[3]"), _driver);
                while (respostaPesquisa.Text != "Sua pesquisa não encontrou nenhum documento correspondente.")
                {
                    int index_nota = 0, index_situacao = 0, index_opcoes = 0;
                    var table = ExtensionsMethods.GetElements(By.XPath("//*[@id=\"FormPaginacao\"]/div[3]/table/tbody"), _driver);
                    var rows = table.First().FindElements(By.TagName("tr"));
                    var ths = rows.First().FindElements(By.TagName("th"));

                    //lê o header da tabela
                    for (int col = 0; col < ths.Count(); col++)
                    {
                        if (ths[col].Text == "No.")
                            index_nota = col;
                        if (ths[col].Text == "Situação")
                            index_situacao = col;
                        if (ths[col].Text == "Opções")
                            index_opcoes = col;
                    }

                    //lê as linhas da tabela (ignora header)
                    for (int row = 1; row < rows.Count(); row++)
                    {
                        var tds = rows[row].FindElements(By.TagName("td"));

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

                            if (respostaPesquisa.Text == "Sua pesquisa não encontrou nenhum documento correspondente.")
                            {
                                _driver.Close();
                                _driver.SwitchTo().Window(parentWindowHandle);
                                return true;
                            }
                            else
                                break;
                        }

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
                    respostaPesquisa = ExtensionsMethods.GetElement(By.XPath("//*[@id=\"FormPaginacao\"]/div[3]"), _driver);
                }
                _driver.Close();
                _driver.SwitchTo().Window(parentWindowHandle);
                return false;
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception(@$" - NFePage (GetResults) - O bot nao foi capaz de encontrar o elemento para interagir - {ex.Message}");
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception(@$" - NFePage (GetResults) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message}");
            }
            catch (ElementNotInteractableException ex)
            {
                throw new Exception(@$" - NFePage (GetResults) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message}");
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$" - NFePage (GetResults) - O bot nao foi capaz de encontrar o elemento para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@" - NFePage (GetResults) - Erro ao aprovar NFe da NFe page - {ex.Message}");
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

                if (ExtensionsMethods.ElementExistInAnotherWindow(By.Id("exibeMsgCertificado"), windowHandle, _driver))
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div[11]/div/button"))).Click();

                _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"nfeNFe\"]/a"))).Click();
                return parentWindowHandle;
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception(@$" - NFePage (NavigateToNFeTab) - O bot nao foi capaz de encontrar o elemento para interagir - {ex.Message}");
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception(@$" - NFePage (NavigateToNFeTab) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message}");
            }
            catch (ElementNotInteractableException ex)
            {
                throw new Exception(@$" - NFePage (NavigateToNFeTab) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message}");
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$" - NFePage (NavigateToNFeTab) - O bot nao foi capaz de encontrar o elemento para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@" - NFePage (NavigateToNFeTab) - Erro ao navegar entre o menu da NFe page - {ex.Message}");
            }
        }

        public void SetFilters(Order order, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var inputInicialPeriod = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("periodoInicial")));
                var inputFinalPeriod = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("periodoFinal")));
                var inputNumberNFe = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("numero")));
                var checkBoxIsAuthorized = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("isAutorizada")));
                var checkBoxIsDenied = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("isDenegada")));
                var checkBoxIsCanceled = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("isCancelada")));
                var checkBoxIsUnused = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("isInutilizada")));
                var checkBoxIsExcluded = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("isExcluida")));
                var buttonAction = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnAcao")));

                ExtensionsMethods.ClearInputElementValue(inputInicialPeriod);
                ExtensionsMethods.ClearInputElementValue(inputFinalPeriod);

                ExtensionsMethods.SendKeysToElement(inputInicialPeriod, order.invoice.date_emission_nf.Date.ToString());
                ExtensionsMethods.SendKeysToElement(inputFinalPeriod, order.invoice.date_emission_nf.Date.ToString());
                ExtensionsMethods.SendKeysToElement(inputNumberNFe, order.invoice.number_nf);

                ExtensionsMethods.ClickInElement(checkBoxIsAuthorized);
                ExtensionsMethods.ClickInElement(checkBoxIsDenied);
                ExtensionsMethods.ClickInElement(checkBoxIsCanceled);
                ExtensionsMethods.ClickInElement(checkBoxIsUnused);
                ExtensionsMethods.ClickInElement(checkBoxIsExcluded);
                ExtensionsMethods.ClickInElement(buttonAction);
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception(@$" - NFePage (SetFilters) - O bot nao foi capaz de encontrar o elemento ({ex.Message.Substring(ex.Message.IndexOf("\"selector\":"), ex.Message.Length - ex.Message.IndexOf("\"selector\":")).Replace("\"selector\":", "").Replace("}", "").Replace("\n  (Session info: chrome=114.0.5735.134)", "")}) para interagir - {ex.Message.Replace("\n", "")}");
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception(@$" - NFePage (SetFilters) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message.Replace("\n", "")}");
            }
            catch (ElementNotInteractableException ex)
            {
                throw new Exception(@$" - NFePage (SetFilters) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message.Replace("\n", "")}");
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$" - NFePage (SetFilters) - O bot nao foi capaz de encontrar o elemento para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@" - NFePage (SetFilters) - Erro ao selecionar os cheackboxes da tela NFe - {ex.Message}");
            }
        }
    }
}
