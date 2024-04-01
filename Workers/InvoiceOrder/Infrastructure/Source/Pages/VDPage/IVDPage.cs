using OpenQA.Selenium;

namespace BloomersWorkers.InvoiceOrder.Infrastructure.Source.Pages
{
    public interface IVDPage
    {
        public void SelectOrder(string nr_pedido, IWebDriver _driver);
        public void SetOrderData(string cnpj, IWebDriver _driver);
        public void SetShippimentData(string cnpj, string cod_transportadora, string volumes, IWebDriver _driver);
        public void SetCostCenter(string cnpj, IWebDriver _driver);
        public bool WaitChaveNFe(IWebDriver _driver);
    }
}
