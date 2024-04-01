using OpenQA.Selenium;

namespace BloomersWorkersCore.Infrastructure.Source.Pages
{
    public interface IHomePage
    {
        public void NavigateToVDOrB2COrNFeOrChangingOrdersScreen(string nr_pedido, IWebDriver _driver);
    }
}
