using BloomersWorkersCore.Domain.Entities;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
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
                var buttonTopBarMenu = ExtensionsMethods.GetElementToBeClickableById("topbar_menu_usuario_navbar_titulo", _wait, Page.TypeEnum.ChangingPassword, "ChangePassword");
                var buttonChangePassword = ExtensionsMethods.GetElementExistsByClassName("topbar-menu-usuario-link-alterar-senha", _wait, Page.TypeEnum.ChangingPassword, "ChangePassword");

                ExtensionsMethods.ClickInElement(buttonTopBarMenu);
                ExtensionsMethods.ClickInElement(buttonChangePassword);

                Thread.Sleep(2 * 1000);

                ExtensionsMethods.ChangeToFrameWhenItsAvaiable("main", _wait, Page.TypeEnum.ChangingPassword, "ChangePassword");

                var inputOldPassword = ExtensionsMethods.GetElementExistsById("senha_antiga", _wait, Page.TypeEnum.ChangingPassword, "ChangePassword");
                var inputNewPassword = ExtensionsMethods.GetElementExistsById("senha_nova", _wait, Page.TypeEnum.ChangingPassword, "ChangePassword");
                var inputNewPassword_2 = ExtensionsMethods.GetElementExistsById("senha_nova2", _wait, Page.TypeEnum.ChangingPassword, "ChangePassword");
                var buttonAlter = ExtensionsMethods.GetElementToBeClickableByClassName("btn-secondary", _wait, Page.TypeEnum.ChangingPassword, "ChangePassword");

                ExtensionsMethods.SendKeysToElement(inputOldPassword, usuario.senha);
                ExtensionsMethods.SendKeysToElement(inputNewPassword, senhaNova);
                ExtensionsMethods.SendKeysToElement(inputNewPassword_2, senhaNova);
                ExtensionsMethods.ClickInElement(buttonAlter);

                usuario.senha = senhaNova;

                Thread.Sleep(1 * 1000);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
