using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages
{
    public interface IVDPage
    {
        public void SelectOrder(string nr_pedido, IWebDriver _driver, WebDriverWait _wait);
        public void SetOrderData(string cnpj, IWebDriver _driver, WebDriverWait _wait);
        public void SetShippimentData(string cnpj, string cod_transportadora, string volumes, IWebDriver _driver, WebDriverWait _wait);
        public void SetCostCenter(string cnpj, IWebDriver _driver, WebDriverWait _wait);
        public bool WaitChaveNFe(IWebDriver _driver, WebDriverWait _wait);
    }
}
