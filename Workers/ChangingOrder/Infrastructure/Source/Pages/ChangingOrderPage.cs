using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkers.ChangingOrder.Infrastructure.Source.Pages
{
    public class ChangingOrderPage : IChangingOrderPage
    {
        public bool AproveOrder(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _driver.FindElement(By.Id("B1")));

                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("B1"))).Click();
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/table[2]/tbody/tr[3]/td[2]/a"))).Click();
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("b_at"))).Click();

                return true;
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (AproveOrder) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message.Replace("\n", "")}");
            }
            catch (ElementNotInteractableException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (AproveOrder) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message.Replace("\n", "")}");
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$" - ChangingOrdersPage (AproveOrder) - O bot nao foi capaz de encontrar para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@" - ChangingOrdersPage (AproveOrder) - Erro ao tentar alterar quantidade produto da lista - {ex.Message}");
            }
        }

        public void ChangeQtdeItemFromList(int cod_produto, int qtde_produto, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var rowsTabelaProdutos = _driver.FindElement(By.Id("tbl_itens")).FindElements(By.TagName("tr"));
                for (int i = 1; i < rowsTabelaProdutos.Count(); i++) //ignora thead
                {
                    if (rowsTabelaProdutos[i].FindElements(By.TagName("td")).First().Text.Contains($"{cod_produto}"))
                    {
                        IWebElement btnEditar = rowsTabelaProdutos[i].FindElements(By.ClassName("iconEdit")).First();
                        btnEditar.Click();

                        var parentWindowHandle = _driver.CurrentWindowHandle;
                        var lstWindows = _driver.WindowHandles.ToList();
                        foreach (var window in lstWindows)
                        {
                            if (window != parentWindowHandle)
                            {
                                _driver.SwitchTo().Window(window);
                                _driver.Manage().Window.Maximize();
                            }
                            else
                                continue;
                        }

                        IWebElement quantidade = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("quantidade")));
                        quantidade.Clear();
                        quantidade.SendKeys($"{qtde_produto}");

                        _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-submit"))).Click();

                        _driver.SwitchTo().Window(parentWindowHandle);
                        _driver.Manage().Window.Maximize();

                        _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("main")));

                        Thread.Sleep(2 * 1000);
                        break;
                    }
                    else
                        continue;
                }
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (ChangeQtdeItemFromList) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message.Replace("\n", "")}");
            }
            catch (ElementNotInteractableException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (ChangeQtdeItemFromList) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message.Replace("\n", "")}");
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$" - ChangingOrdersPage (ChangeQtdeItemFromList) - O bot nao foi capaz de encontrar para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@" - ChangingOrdersPage (ChangeQtdeItemFromList) - Erro ao tentar alterar quantidade produto da lista - {ex.Message}");
            }
        }

        public void RemoveItemFromList(int cod_produto, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var rowsTabelaProdutos = _driver.FindElement(By.Id("tbl_itens")).FindElements(By.TagName("tr"));
                for (int i = 1; i < rowsTabelaProdutos.Count(); i++) //ignora thead
                {
                    if (rowsTabelaProdutos[i].FindElements(By.TagName("td")).First().Text.Contains($"{cod_produto}"))
                    {
                        IWebElement btnExcluir = rowsTabelaProdutos[i].FindElements(By.ClassName("lixeiraOn")).First();
                        btnExcluir.Click();
                        Thread.Sleep(2 * 1000);
                        break;
                    }
                    else
                        continue;
                }
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (SelectOrder) - O bot nao foi capaz de encontrar o elemento para interagir - {ex.Message}");
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (RemoveItemFromList) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message}");
            }
            catch (ElementNotInteractableException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (RemoveItemFromList) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message}");
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$" - ChangingOrdersPage (RemoveItemFromList) - O bot nao foi capaz de encontrar para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@" - ChangingOrdersPage (RemoveItemFromList) - Erro ao tentar remover produto da lista - {ex.Message}");
            }
        }

        public void SelectOrder(string nr_pedido, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                Thread.Sleep(2 * 1000);

                _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("main")));
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("orcamento"))).SendKeys($"{nr_pedido.Replace("MI-", "").Replace("OA-", "").Replace("VD", "").Replace("LJ", "")}");
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("B1"))).Click();
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[4]/div[3]/div/button"))).Click();
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("B1"))).Click();

                Thread.Sleep(2 * 1000);
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (SelectOrder) - O bot nao foi capaz de encontrar para interagir - {ex.Message}");
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (SelectOrder) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message}");
            }
            catch (ElementNotInteractableException ex)
            {
                throw new Exception(@$" - ChangingOrdersPage (SelectOrder) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message}");
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new Exception(@$" - ChangingOrdersPage (SelectOrder) - O bot nao foi capaz de encontrar para interagir - {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($@" - ChangingOrdersPage (SelectOrder) - Erro ao selecionar pedido para ser alterado - {ex.Message}");
            }
        }
    }
}
