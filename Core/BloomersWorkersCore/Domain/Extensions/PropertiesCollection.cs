using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkersCore.Domain.Extensions
{
    public enum TypeBy
    {
        Id,
        Name,
        TagName,
        ClassName,
        Xpath
    }

    public class PropertiesCollection
    {
        //public static IWebDriver _driver { get; set; }
        public static WebDriverWait _wait { get; set; }
    }
}
