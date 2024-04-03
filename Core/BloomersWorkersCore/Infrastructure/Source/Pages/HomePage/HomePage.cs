using BloomersWorkersCore.Domain.CustomExceptions;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

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

                var modal = ExtensionsMethods.GetElementExixtsById("modalNotasPendentes", _wait, Page.TypeEnum.Home, "ClosePendingInvoicesModal");
                   
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
                var buttonSalesPanel = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"liModulo_3\"]/ul/li[2]", _wait, Page.TypeEnum.Home, "NavigateToB2CScreen");

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
                var buttonQuoteOrder = ExtensionsMethods.GetElementExixtsByXpath("//*[@id=\"liModulo_10\"]/ul/li[2]/a", _wait, Page.TypeEnum.Home, "NavigateToChangingOrdersScreen");
                var buttonChangeQuoteOrder = ExtensionsMethods.GetElementExixtsByXpath("//*[@id=\"liModulo_10\"]/ul/li[2]/ul/li[3]/a", _wait, Page.TypeEnum.Home, "NavigateToChangingOrdersScreen");

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
                var buttonStartNFe = ExtensionsMethods.GetElementToBeClickableByXpath("//*[@id=\"liModulo_12\"]/ul/li/a", _wait, Page.TypeEnum.Home, "NavigateToNFeScreen");

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
                var buttonInvoiceOrder = ExtensionsMethods.GetElementExixtsByXpath("//*[@id=\"liModulo_10\"]/ul/li[5]/ul/li[2]/a", _wait, Page.TypeEnum.Home, "NavigateToVDScreen");
                var buttonInvoice = ExtensionsMethods.GetElementExixtsByXpath("//a[@title='Nota Fiscal']", _wait, Page.TypeEnum.Home, "NavigateToVDScreen");
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
    }
}
