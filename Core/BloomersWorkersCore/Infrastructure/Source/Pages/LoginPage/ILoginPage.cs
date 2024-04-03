using BloomersWorkersCore.Domain.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkersCore.Infrastructure.Source.Pages
{
    public interface ILoginPage
    {
        public void InsertLoginAndPassword(MicrovixUser microvixUser, WebDriverWait _wait);
        public void SelectCompany(string cnpj, MicrovixUser microvixUser, IWebDriver _driver, WebDriverWait _wait);
    }
}
