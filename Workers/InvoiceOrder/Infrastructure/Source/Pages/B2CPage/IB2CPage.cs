using OpenQA.Selenium;

namespace BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages
{
    public interface IB2CPage
    {
        public void SelectOrder(string nr_pedido, IWebDriver _driver);
        public bool SetOrderData(string transportadora, string cnpj, string nr_pedido, IWebDriver _driver);
    }
}
