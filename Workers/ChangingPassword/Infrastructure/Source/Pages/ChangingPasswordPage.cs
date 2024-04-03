using BloomersWorkersCore.Domain.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkers.ChangingPassword.Infrastructure.Source.Pages
{
    public class ChangingPasswordPage : IChangingPasswordPage
    {
        public bool ChangePassword(MicrovixUser usuario, string senhaNova, WebDriverWait _wait)
        {
            try
            {
                if (_wait.Until(ExpectedConditions.ElementExists(By.Id("modalNotasPendentes"))).Displayed && _wait.Until(ExpectedConditions.ElementExists(By.Id("modalNotasPendentes"))).Enabled)
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"modalNotasPendentes\"]/div/div/div[3]/button"))).Click();

                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("topbar_menu_usuario_navbar_titulo"))).Click();
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("topbar-menu-usuario-link-alterar-senha"))).Click();

                Thread.Sleep(2 * 1000);

                _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("main")));
                _wait.Until(ExpectedConditions.ElementExists(By.Id("senha_antiga"))).SendKeys(usuario.senha);
                _wait.Until(ExpectedConditions.ElementExists(By.Id("senha_nova"))).SendKeys(senhaNova);
                _wait.Until(ExpectedConditions.ElementExists(By.Id("senha_nova2"))).SendKeys(senhaNova);
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-secondary"))).Click();

                usuario.senha = senhaNova;
                return true;
            }
            catch (NoSuchElementException ex)
            {
                if (ex.Message.Contains("no such element: Unable to locate element: "))
                    throw new Exception(@$" - HomePage (TrocaSenha) - O bot nao foi capaz de encontrar o elemento ({ex.Message.Substring(ex.Message.IndexOf("\"selector\":"), ex.Message.Length - ex.Message.IndexOf("\"selector\":")).Replace("\"selector\":", "").Replace("}", "").Replace("\n  (Session info: chrome=114.0.5735.134)", "")}) para interagir - {ex.Message.Replace("\n", "")}");
                throw new Exception($@" - HomePage (TrocaSenha) - Erro ao trocar senha no menu da home page - {ex.Message.Replace("\n", "")}");
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception(@$" - HomePage (TrocaSenha) - O bot nao foi capaz de clicar no botão, o botão pode estar: desabilitado, ainda não carregado, coberto com alguma modal, ou não localizado através das cordenadas - {ex.Message.Replace("\n", "")}");
            }
            catch (ElementNotInteractableException ex)
            {
                throw new Exception(@$" - HomePage (TrocaSenha) - O bot nao foi capaz de interagir com elemento, o elemento pode estar: não visivel, não exibido, fora da tela, ou coberto com alguma modal - {ex.Message.Replace("\n", "")}");
            }
            catch (Exception ex)
            {
                throw new Exception($@" - HomePage (TrocaSenha) - Erro ao trocar senha no menu da home page - {ex.Message.Replace("\n", "")}");
            }
        }
    }
}
