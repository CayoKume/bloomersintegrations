using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Data.Common;

namespace BloomersWorkersCore.Infrastructure.Source.Drivers
{
    public class Driver : IDriver
    {
        private IWebDriver? _driver;

        public void Dispose()
        {
            _driver?.Close();
            _driver?.Dispose();
            _driver?.Quit();
        }

        public IWebDriver GetChromeDriverInstance()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("--allow-running-insecure-content");
            chromeOptions.AddArgument("--user-agent=Chrome/80.0.3987.132");
            chromeOptions.AddArgument("--headless=new");
            chromeOptions.AddArgument("--disable-crash-reporter");
            _driver = new OpenQA.Selenium.Chrome.ChromeDriver(driverService, chromeOptions);
            _driver.Url = $"https://erp.microvix.com.br/";
            return _driver;
        }

        public IWebDriver GetEdgeDriverInstance()
        {
            var driverService = EdgeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var edgeOptions = new EdgeOptions();
            edgeOptions.AddArgument("--no-sandbox");
            edgeOptions.AddArgument("--start-maximized");
            //edgeOptions.AddArgument("--headless=new");
            edgeOptions.AddArgument("--disable-crash-reporter");
            _driver = new OpenQA.Selenium.Edge.EdgeDriver(edgeOptions);
            _driver.Url = $"https://erp.microvix.com.br/";
            return _driver;
        }

        public WebDriverWait GetWebDriverWaitInstance(IWebDriver _driver)
        {
            return new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        }
    }
}
