using OpenQA.Selenium;

namespace BloomersWorkersCore.Infrastructure.Source.Drivers
{
    public interface IChromeDriver
    {
        public IWebDriver GetChromeDriverInstance();
    }
}
