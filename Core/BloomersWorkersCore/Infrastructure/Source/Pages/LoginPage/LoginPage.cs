using BloomersWorkersCore.Domain.CustomExceptions;
using BloomersWorkersCore.Domain.Entities;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkersCore.Infrastructure.Source.Pages
{
    public class LoginPage : ILoginPage
    {
        private readonly IConfiguration _configuration;

        public LoginPage(IConfiguration configuration) =>
            (_configuration) = (configuration);

        public void Login(MicrovixUser microvixUser, IWebDriver _driver)
        {
            try
            {
                var botName = $"{_configuration.GetSection("ConfigureService").GetSection("BotName").Value} {_configuration.GetSection("ConfigureService").GetSection("FinalIdControle").Value}";
                var comboBoxSelectByText = string.Empty;
                var valueSelectByValue = string.Empty;

                if (botName.Contains("Gabot"))
                {
                    comboBoxSelectByText = "5 - Open Era - Ecommerce";
                    valueSelectByValue = "5";
                }
                else if (botName.Contains("Vanabot"))
                {
                    comboBoxSelectByText = "1 - Misha - Ecommerce";
                    valueSelectByValue = "1";
                }

                PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_login"))).SendKeys(microvixUser.usuario);
                PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_senha"))).SendKeys(microvixUser.senha);
                PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-secondary"))).SendKeys(Keys.Enter);

                PropertiesCollection._wait.Until(ExpectedConditions.ElementIsVisible(By.Id("quantidade_empresa")));
                IWebElement selecaoEmpresa = PropertiesCollection._wait.Until(ExpectedConditions.ElementExists(By.Id("sel_empresa_portal_usuario")));
                PropertiesCollection._wait.Until(ExpectedConditions.TextToBePresentInElement(selecaoEmpresa, comboBoxSelectByText));

                SelectElement comboboxEmpresa = new SelectElement(selecaoEmpresa);
                _driver.SwitchTo().DefaultContent();

                comboboxEmpresa.SelectByText(comboBoxSelectByText);
                IWebElement btnSelecionar = PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnselecionar_empresa")));
                btnSelecionar.Click();
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"LoginPage (Login) - O bot nao foi capaz de encontrar ou interagir com o elemento, o elemento pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não contido na pagina", 
                    ex.InnerException.Message.Substring(0, ex.InnerException.Message.IndexOf("(Session")),
                    Page.TypeEnum.Login
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"LoginPage (Login) - Erro ao tentar efetuar o login - {ex.Message}");
            }
        }

        public void Login(string cnpj, MicrovixUser microvixUser, IWebDriver _driver)
        {
            try
            {
                var botName = $"{_configuration.GetSection("ConfigureService").GetSection("BotName").Value} {_configuration.GetSection("ConfigureService").GetSection("FinalIdControle").Value}";
                var comboBoxSelectByText = string.Empty;
                var valueSelectByValue = string.Empty;

                if (cnpj == "42538267000268")
                {
                    comboBoxSelectByText = "5 - Open Era - Ecommerce";
                    valueSelectByValue = "5";
                }
                else if (cnpj == "38367316000199")
                {
                    comboBoxSelectByText = "1 - Misha - Ecommerce";
                    valueSelectByValue = "1";
                }
                else if (cnpj == "38367316000865")
                {
                    comboBoxSelectByText = "17 - MISHA - Volo";
                    valueSelectByValue = "14";
                }

                if (ExtensionsMethods.ElementExist(By.Id("form_login"), _driver))
                {
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_login"))).SendKeys(microvixUser.usuario);
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_senha"))).SendKeys(microvixUser.senha);
                    PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-secondary"))).SendKeys(Keys.Enter);

                    PropertiesCollection._wait.Until(ExpectedConditions.ElementIsVisible(By.Id("quantidade_empresa")));
                    IWebElement selecaoEmpresa = PropertiesCollection._wait.Until(ExpectedConditions.ElementExists(By.Id("sel_empresa_portal_usuario")));
                    PropertiesCollection._wait.Until(ExpectedConditions.TextToBePresentInElement(selecaoEmpresa, comboBoxSelectByText));

                    SelectElement comboboxEmpresa = new SelectElement(selecaoEmpresa);
                    _driver.SwitchTo().DefaultContent();

                    comboboxEmpresa.SelectByText(comboBoxSelectByText);
                    IWebElement btnSelecionar = PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnselecionar_empresa")));
                    btnSelecionar.Click();
                }
                else if (ExtensionsMethods.ElementExist(By.Id("topbar_sel_empresa_portal_usuario"), _driver))
                {
                    SelectElement dropdownMenu = new SelectElement(_driver.FindElement(By.Id("topbar_sel_empresa_portal_usuario")));
                    dropdownMenu.SelectByValue(valueSelectByValue);
                }
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            { 
                throw new CustomNoSuchElementException(
                    @$"LoginPage (Login) - O bot nao foi capaz de encontrar ou interagir com o elemento, o elemento pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não contido na pagina", 
                    ex.InnerException.Message.Substring(0, ex.InnerException.Message.IndexOf("(Session")),
                    Page.TypeEnum.Login
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"LoginPage (Login) - Erro ao tentar efetuar o login - {ex.Message}");
            }
        }

        //public void Login(MicrovixUser usuario)
        //{
        //    try
        //    {
        //        if (ExtensionsMethods.ElementExist(By.Id("form_login")))
        //        {
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_login"))).SendKeys(usuario.usuario);
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_senha"))).SendKeys(usuario.senha);
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-secondary"))).SendKeys(Keys.Enter);

        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementIsVisible(By.Id("quantidade_empresa")));
        //            IWebElement selecaoEmpresa = PropertiesCollection._wait.Until(ExpectedConditions.ElementExists(By.Id("sel_empresa_portal_usuario")));
        //            PropertiesCollection._wait.Until(ExpectedConditions.TextToBePresentInElement(selecaoEmpresa, "1 - Misha - Ecommerce"));

        //            SelectElement comboboxEmpresa = new SelectElement(selecaoEmpresa);
        //            PropertiesCollection._driver.SwitchTo().DefaultContent();

        //            comboboxEmpresa.SelectByText("1 - Misha - Ecommerce");
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnselecionar_empresa"))).Click();
        //        }
        //        else
        //        {
        //            PropertiesCollection._driver.SwitchTo().DefaultContent();
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("topbar_menu_usuario_navbar_titulo"))).Click();
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("topbar-menu-usuario-link-sair"))).Click();
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[13]/div/div[3]/button[1]"))).Click();

        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_login"))).SendKeys(usuario.usuario);
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("f_senha"))).SendKeys(usuario.senha);
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-secondary"))).SendKeys(Keys.Enter);

        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementIsVisible(By.Id("quantidade_empresa")));
        //            IWebElement selecaoEmpresa = PropertiesCollection._wait.Until(ExpectedConditions.ElementExists(By.Id("sel_empresa_portal_usuario")));
        //            PropertiesCollection._wait.Until(ExpectedConditions.TextToBePresentInElement(selecaoEmpresa, "1 - Misha - Ecommerce"));

        //            SelectElement comboboxEmpresa = new SelectElement(selecaoEmpresa);
        //            PropertiesCollection._driver.SwitchTo().DefaultContent();

        //            comboboxEmpresa.SelectByText("1 - Misha - Ecommerce");
        //            PropertiesCollection._wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnselecionar_empresa"))).Click();
        //        }
        //    }
        //    catch (NoSuchElementException ex)
        //    {
        //        throw new Exception(@$" - LoginPage (Login) - O bot nao foi capaz de encontrar o elemento para interagir, o elemento pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não contido na pagina - {ex.Message}");
        //    }
        //    catch (ElementClickInterceptedException ex)
        //    {
        //        throw new Exception(@$" - LoginPage (Login) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message}");
        //    }
        //    catch (ElementNotInteractableException ex)
        //    {
        //        throw new Exception(@$" - LoginPage (Login) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message}");
        //    }
        //    catch (Exception ex) when (ex.Message.Contains("Timed out"))
        //    {
        //        throw new Exception(@$" - LoginPage (Login) - O bot nao foi capaz de encontrar ou interagir com o elemento, o elemento pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não contido na pagina - {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($@" - LoginPage (Login) - Erro ao tentar efetuar o login - {ex.Message}");
        //    }
        //}
    }
}
