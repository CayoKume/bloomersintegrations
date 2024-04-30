using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkersCore.Infrastructure.Source.Drivers
{
    public interface IDriver
    {
        public IWebDriver GetChromeDriverInstance();
        public Tuple<IWebDriver, int> GetEdgeDriverInstance();
        public WebDriverWait GetWebDriverWaitInstance(IWebDriver _driver);
        public void Dispose(int pid);
        public void Dispose();
    }
}
