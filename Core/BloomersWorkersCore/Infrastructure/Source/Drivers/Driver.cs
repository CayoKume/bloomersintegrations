using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

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

        public void Dispose(int pid)
        {
            _driver?.Close();
            _driver?.Dispose();
            _driver?.Quit();
            Process.GetProcessById(pid).Kill();
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
            //chromeOptions.AddArgument("--headless=new");
            chromeOptions.AddArgument("--disable-crash-reporter");
            _driver = new OpenQA.Selenium.Chrome.ChromeDriver(driverService, chromeOptions);
            _driver.Url = $"https://erp.microvix.com.br/";
            return _driver;
        }

        public Tuple<IWebDriver, int> GetEdgeDriverInstance()
        {
            var driverService = EdgeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var pid = driverService.ProcessId;
            var edgeOptions = new EdgeOptions();
            edgeOptions.AddArgument("--no-sandbox");
            edgeOptions.AddArgument("--start-maximized");
            edgeOptions.AddArgument("--ignore-certificate-errors");
            edgeOptions.AddArgument("--allow-running-insecure-content");
            edgeOptions.AddArgument("--user-agent=Chrome/80.0.3987.132");
            //edgeOptions.AddArgument("--headless=new");
            edgeOptions.AddArgument("--disable-crash-reporter");
            edgeOptions.AddArgument("user-data-dir=C:\\Users\\Adm-NewBloomers\\AppData\\Local\\Microsoft\\Edge\\User Data1");
            _driver = new OpenQA.Selenium.Edge.EdgeDriver(edgeOptions);
            _driver.Url = $"https://erp.microvix.com.br/";

            return new Tuple<IWebDriver, int>(_driver, pid);
        }

        public WebDriverWait GetWebDriverWaitInstance(IWebDriver _driver)
        {
            return new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        }
    }
}
