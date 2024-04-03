using BloomersWorkers.AuthorizeNFe.Domain.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkers.AuthorizeNFe.Infrastructure.Source.Pages
{
    public interface IAuthorizeNFePage
    {
        public string NavigateToNFeTab(IWebDriver _driver, WebDriverWait _wait);
        public void SetFilters(Order order, IWebDriver _driver, WebDriverWait _wait);
        public bool GetResults(string parentWindowHandle, IWebDriver _driver);
    }
}
