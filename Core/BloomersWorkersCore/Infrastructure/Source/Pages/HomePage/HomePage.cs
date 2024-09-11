using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkersCore.Infrastructure.Source.Pages
{
    public class HomePage : IHomePage
    {
        public void ClosePendingInvoicesModal(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                Thread.Sleep(2 * 1000);

                _driver.SwitchTo().DefaultContent();

                var modal = ExtensionsMethods.GetElementExistsById("modalNotasPendentes", _wait, Page.TypeEnum.Home, "ClosePendingInvoicesModal");
                   
                if (modal.Displayed && modal.Enabled)
                {
                    var buttonCloseModal = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"modalNotasPendentes\"]/div/div/div[3]/button", _wait, Page.TypeEnum.Home, "ClosePendingInvoicesModal");
                    ExtensionsMethods.ClickInElement(buttonCloseModal);
                }
            }
            catch
            {
                throw;
            }
        }

        public void OpenSideMenu(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var iconMenu = ExtensionsMethods.GetElementToBeClickableById("iconMenu", _wait, Page.TypeEnum.Home, "OpenSideMenu");
                var buttonMenu = ExtensionsMethods.GetElementToBeClickableById("frente-logo-hamburger", _wait, Page.TypeEnum.Home, "OpenSideMenu");

                if (iconMenu.GetAttribute("title") == "Expandir menu")
                    ExtensionsMethods.ClickInElement(buttonMenu);
            }
            catch
            {
                throw;
            }
        }

        public void NavigateToB2CScreen(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var buttonB2C = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"liModulo_3\"]", _wait, Page.TypeEnum.Home, "NavigateToB2CScreen");
                var buttonSalesPanel = ExtensionsMethods.GetElementExistsByXpath("//*[@id=\"liModulo_3\"]/ul/li[2]", _wait, Page.TypeEnum.Home, "NavigateToB2CScreen");

                ExtensionsMethods.ClickInElement(buttonB2C);
                ExtensionsMethods.ClickInElement(buttonSalesPanel);
            }
            catch
            {
                throw;
            }
        }

        public void NavigateToChangingOrdersScreen(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var buttonInvoicing = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"liModulo_10\"]", _wait, Page.TypeEnum.Home, "NavigateToChangingOrdersScreen");
                var buttonQuoteOrder = ExtensionsMethods.GetElementExistsByXpath("//*[@id=\"liModulo_10\"]/ul/li[2]/a", _wait, Page.TypeEnum.Home, "NavigateToChangingOrdersScreen");
                var buttonChangeQuoteOrder = ExtensionsMethods.GetElementExistsByXpath("//*[@id=\"liModulo_10\"]/ul/li[2]/ul/li[3]/a", _wait, Page.TypeEnum.Home, "NavigateToChangingOrdersScreen");

                if (!buttonQuoteOrder.Displayed)
                {
                    ExtensionsMethods.ClickInElement(buttonInvoicing);
                    ExtensionsMethods.ClickInElement(buttonQuoteOrder);
                }

                ExtensionsMethods.ClickInElement(buttonChangeQuoteOrder);
            }
            catch
            {
                throw;
            }
        }

        public void NavigateToNFeScreen(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var buttonNFe = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"liModulo_12\"]/a", _wait, Page.TypeEnum.Home, "NavigateToNFeScreen");
                var buttonStartNFe = ExtensionsMethods.GetElementExistsByXpath("//*[@id=\"liModulo_12\"]/ul/li/a", _wait, Page.TypeEnum.Home, "NavigateToNFeScreen");

                ExtensionsMethods.ClickInElement(buttonNFe);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", buttonStartNFe);
                ExtensionsMethods.ClickInElement(buttonStartNFe);
            }
            catch
            {
                throw;
            }
        }

        public void NavigateToVDScreen(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var buttonInvoicing = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"liModulo_10\"]", _wait, Page.TypeEnum.Home, "NavigateToVDScreen");
                var buttonInvoiceOrder = ExtensionsMethods.GetElementExistsByXpath("//*[@id=\"liModulo_10\"]/ul/li[5]/ul/li[2]/a", _wait, Page.TypeEnum.Home, "NavigateToVDScreen");
                var buttonInvoice = ExtensionsMethods.GetElementExistsByXpath("//a[@title='Nota Fiscal']", _wait, Page.TypeEnum.Home, "NavigateToVDScreen");
                var buttonMySales = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"liModulo_8\"]/a", _wait, Page.TypeEnum.Home, "NavigateToVDScreen");

                if (!buttonInvoiceOrder.Displayed)
                {
                    ExtensionsMethods.ClickInElement(buttonInvoicing);
                    ExtensionsMethods.ClickInElement(buttonInvoice);
                }

                //scroll to my sales just for the buttonInvoiceOrder turn visible
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", buttonMySales);
                ExtensionsMethods.ClickInElement(buttonInvoiceOrder);
            }
            catch
            {
                throw;
            }
        }

        public void Logout(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                _driver.SwitchTo().DefaultContent();

                var buttonTopBarMenu = ExtensionsMethods.GetElementToBeClickableById("topbar_menu_usuario_navbar_titulo", _wait, Page.TypeEnum.Home, "Logout");
                var buttonLogout = ExtensionsMethods.GetElementExistsByClassName("topbar-menu-usuario-link-sair", _wait, Page.TypeEnum.Home, "Logout");

                ExtensionsMethods.ClickInElement(buttonTopBarMenu);
                ExtensionsMethods.ClickInElement(buttonLogout);

                Thread.Sleep(2 * 1000);

                var buttoConfirm = ExtensionsMethods.GetElementExistsByClassName("swal2-confirm", _wait, Page.TypeEnum.Home, "Logout");
                ExtensionsMethods.ClickInElement(buttoConfirm);

                Thread.Sleep(1 * 1000);
            }
            catch
            {
                throw;
            }
        }
    }
}
