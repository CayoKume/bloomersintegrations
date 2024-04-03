
using BloomersWorkers.ChangingPassword.Infrastructure.Repositorys;
using BloomersWorkers.ChangingPassword.Infrastructure.Source.Pages;
using BloomersWorkersCore.Domain.Entities;
using BloomersWorkersCore.Infrastructure.Source.Drivers;
using BloomersWorkersCore.Infrastructure.Source.Pages;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkers.ChangingPassword.Application.Services
{
    public class ChangingPasswordService : IChangingPasswordService
    {
        private readonly ILoginPage _loginPage;
        private readonly IChromeDriver _chromeDriver;
        private readonly IChangingPasswordPage _changingPasswordPage;
        private readonly IChangingPasswordRepository _changingPasswordRepository;

        public ChangingPasswordService (ILoginPage loginPage, IChromeDriver chromeDriver, IChangingPasswordPage changingPasswordPage, IChangingPasswordRepository changingPasswordRepository) =>
            (_loginPage, _chromeDriver, _changingPasswordPage, _changingPasswordRepository) = (loginPage, chromeDriver, changingPasswordPage, changingPasswordRepository);

        public async Task ChangePassword()
        {
            try
            {
                var listUsuarios = await _changingPasswordRepository.GetMicrovixUser();
                if (listUsuarios.Count() > 0)
                {
                    using (var driver = _chromeDriver.GetChromeDriverInstance())
                    {
                        var wait = _chromeDriver.GetWebDriverWaitInstance(driver);

                        foreach (var usuario in listUsuarios)
                        {
                            if (ChangePassword(usuario, wait))
                            {
                                _changingPasswordRepository.UpdateLastupdateonFromMicrovixUsers(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool ChangePassword(MicrovixUser usuario, WebDriverWait wait)
        {
            try
            {
                var senhaNova = $"Misha@{Convert.ToInt64(usuario.senha.Replace("Misha@", "")) + 1}";
                return _changingPasswordPage.ChangePassword(usuario, senhaNova, wait);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
