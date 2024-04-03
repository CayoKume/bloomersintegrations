using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkers.ChangingOrder.Infrastructure.Source.Pages
{
    public interface IChangingOrderPage
    {
        public void SelectOrder(string nr_pedido, IWebDriver _driver, WebDriverWait _wait);
        public void ConfirmOrder(IWebDriver _driver, WebDriverWait _wait);
        public void RemoveItemFromList(int cod_produto, IWebDriver _driver, WebDriverWait _wait);
        public void ChangeQtdeItemFromList(int cod_produto, int qtde_produto, IWebDriver _driver, WebDriverWait _wait);
        public bool AproveOrder(IWebDriver _driver, WebDriverWait _wait);
        public void ProceedOrder(IWebDriver _driver, WebDriverWait _wait);
    }
}
