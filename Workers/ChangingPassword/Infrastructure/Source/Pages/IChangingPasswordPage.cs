using BloomersWorkersCore.Domain.Entities;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkers.ChangingPassword.Infrastructure.Source.Pages
{
    public interface IChangingPasswordPage
    {
        public bool ChangePassword(MicrovixUser usuario, string senhaNova, WebDriverWait _wait);
    }
}
