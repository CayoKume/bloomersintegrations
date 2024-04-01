using BloomersWorkersCore.Domain.Entities;
using OpenQA.Selenium;

namespace BloomersWorkersCore.Infrastructure.Source.Pages
{
    public interface ILoginPage
    {
        //public void Login();
        public void Login(string cnpj, MicrovixUser microvixUser, IWebDriver _driver);
        public void Login(MicrovixUser usuario, IWebDriver _driver);
    }
}
