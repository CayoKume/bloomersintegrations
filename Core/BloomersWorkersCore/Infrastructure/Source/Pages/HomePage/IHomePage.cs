using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkersCore.Infrastructure.Source.Pages
{
    public interface IHomePage
    {
        public void ClosePendingInvoicesModal(IWebDriver _driver, WebDriverWait _wait);
        public void OpenSideMenu(IWebDriver _driver, WebDriverWait _wait);
        public void NavigateToChangingOrdersScreen(IWebDriver _driver, WebDriverWait _wait);
        public void NavigateToNFeScreen(IWebDriver _driver, WebDriverWait _wait);
        public void NavigateToB2CScreen(IWebDriver _driver, WebDriverWait _wait);
        public void NavigateToVDScreen(IWebDriver _driver, WebDriverWait _wait);
    }
}
