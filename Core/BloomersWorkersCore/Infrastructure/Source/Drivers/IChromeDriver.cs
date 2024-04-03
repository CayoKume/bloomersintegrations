using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkersCore.Infrastructure.Source.Drivers
{
    public interface IChromeDriver
    {
        public IWebDriver GetChromeDriverInstance();
        public WebDriverWait GetWebDriverWaitInstance(IWebDriver _driver);
    }
}
