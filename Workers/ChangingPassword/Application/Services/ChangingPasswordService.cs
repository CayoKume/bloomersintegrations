
using BloomersWorkers.ChangingPassword.Infrastructure.Repositorys;
using BloomersWorkers.ChangingPassword.Infrastructure.Source.Pages;
using BloomersWorkersCore.Domain.Entities;
using BloomersWorkersCore.Infrastructure.Source.Drivers;
using BloomersWorkersCore.Infrastructure.Source.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkers.ChangingPassword.Application.Services
{
    public class ChangingPasswordService : IChangingPasswordService
    {
        private readonly ILoginPage _loginPage;
        private readonly IHomePage _homePage;
        private readonly IDriver _chromeDriver;
        private readonly IChangingPasswordPage _changingPasswordPage;
        private readonly IChangingPasswordRepository _changingPasswordRepository;

        public ChangingPasswordService (ILoginPage loginPage, IHomePage homePage, IDriver chromeDriver, IChangingPasswordPage changingPasswordPage, IChangingPasswordRepository changingPasswordRepository) =>
            (_loginPage, _homePage, _chromeDriver, _changingPasswordPage, _changingPasswordRepository) = (loginPage, homePage, chromeDriver, changingPasswordPage, changingPasswordRepository);

        public async Task ChangePassword()
        {
            try
            {
                var users = await _changingPasswordRepository.GetMicrovixUser();

                if (users.Count() > 0)
                {
                    using (var driver = _chromeDriver.GetChromeDriverInstance())
                    {
                        var wait = _chromeDriver.GetWebDriverWaitInstance(driver);

                        for (int i = 0; i < users.Count(); i ++)
                        {
                            if (i == 0)
                            {
                                _loginPage.InsertLoginAndPassword(users[i], wait);
                                _loginPage.SelectCompany("38367316000199", driver, wait);
                            }
                            else
                                _loginPage.SelectCompanyFromTopBar("38367316000199", wait);
                            
                            if (ChangePassword(users[i], driver, wait))
                                await _changingPasswordRepository.UpdateLastupdateonFromMicrovixUsers(users[i]);

                            _homePage.Logout(driver, wait);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool ChangePassword(MicrovixUser usuario, IWebDriver _driver, WebDriverWait wait)
        {
            try
            {
                _homePage.ClosePendingInvoicesModal(_driver, wait);
                _homePage.OpenSideMenu(_driver, wait);

                var senhaNova = $"Misha@{Convert.ToInt64(usuario.senha.Replace("Misha@", "")) + 1}";
                return _changingPasswordPage.ChangePassword(usuario, senhaNova, wait);
            }
            catch
            {
                throw;
            }
        }
    }
}
