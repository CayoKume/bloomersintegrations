using BloomersWorkersCore.Domain.CustomExceptions;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkersCore.Infrastructure.Source.Pages
{
    public class HomePage : IHomePage
    {
        public void NavigateToVDOrB2COrNFeOrChangingOrdersScreen(string nr_pedido, IWebDriver _driver)
        {
            try
            {
                _driver.SwitchTo().DefaultContent();

                Thread.Sleep(2 * 1000);
                
                if (PropertiesCollection._wait.Until(ExpectedConditions.ElementExists(By.Id("modalNotasPendentes"))).Displayed && PropertiesCollection._wait.Until(ExpectedConditions.ElementExists(By.Id("modalNotasPendentes"))).Enabled)
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"modalNotasPendentes\"]/div/div/div[3]/button"))).Click();

                IWebElement menu = PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("iconMenu")));

                if (menu.GetAttribute("title") == "Expandir menu")
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("frente-logo-hamburger"))).Click();

                if (nr_pedido.Equals("NFe"))
                {
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_12\"]/a"))).Click();
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _driver.FindElement(By.XPath("//*[@id=\"liModulo_12\"]/ul/li/a")));
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_12\"]/ul/li/a"))).Click();
                }

                else if (nr_pedido.Equals("Orcamento/Pedido"))
                {
                    if (!PropertiesCollection._wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"liModulo_10\"]/ul/li[2]/ul/li[2]/a"))).Displayed)
                    {
                        PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_10\"]"))).Click();
                        PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_10\"]/ul/li[2]/a"))).Click();
                    }

                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_10\"]/ul/li[2]/ul/li[3]/a"))).Click();
                }

                else if (nr_pedido.Contains("-VD") || nr_pedido.Contains("-LJ"))
                {
                    if (!PropertiesCollection._wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"liModulo_10\"]/ul/li[5]/ul/li[2]/a"))).Displayed)
                    {
                        PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_10\"]"))).Click();
                        PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@title='Nota Fiscal']"))).Click();
                    }

                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _driver.FindElement(By.XPath("//*[@id=\"liModulo_8\"]/a")));
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_10\"]/ul/li[5]/ul/li[2]/a"))).Click();
                }

                else
                {
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_3\"]"))).Click();
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"liModulo_3\"]/ul/li[2]"))).Click();
                }
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"HomePage (NavigateToVDOrB2COrNFeOrChangingOrdersScreen) - O bot nao foi capaz de encontrar o elemento para interagir", 
                    ex.InnerException.Message,
                    Page.TypeEnum.Home  
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"HomePage (NavigateToVDOrB2COrNFeOrChangingOrdersScreen) - Erro ao navegar entre o menu da home page - {ex.InnerException.Message}");
            }
        }
    }
}
