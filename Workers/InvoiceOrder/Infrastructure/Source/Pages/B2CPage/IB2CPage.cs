using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages
{
    public interface IB2CPage
    {
        public void SelectOrder(string nr_pedido, IWebDriver _driver, WebDriverWait _wait);
        public bool SetOrderData(string transportadora, string cnpj, string nr_pedido, IWebDriver _driver, WebDriverWait _wait);
    }
}
