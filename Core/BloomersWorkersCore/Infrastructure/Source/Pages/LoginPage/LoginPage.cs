using BloomersWorkersCore.Domain.Entities;
using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BloomersWorkersCore.Infrastructure.Source.Pages
{
    public class LoginPage : ILoginPage
    {
        public void InsertLoginAndPassword(MicrovixUser microvixUser, WebDriverWait _wait)
        {
            try
            {
                var flagFormLogin = ExtensionsMethods.ChecksIfElementExixtsById("form_login", _wait, Page.TypeEnum.Login, "InsertLoginAndPassword");
                var inputLogin = ExtensionsMethods.GetElementToBeClickableById("f_login", _wait, Page.TypeEnum.Login, "InsertLoginAndPassword");
                var inputPassword = ExtensionsMethods.GetElementToBeClickableById("f_senha", _wait, Page.TypeEnum.Login, "InsertLoginAndPassword");
                var buttonEnter = ExtensionsMethods.GetElementToBeClickableByClassName("btn-secondary", _wait, Page.TypeEnum.Login, "InsertLoginAndPassword");

                if (flagFormLogin)
                {
                    ExtensionsMethods.SendKeysToElement(inputLogin, microvixUser.usuario);
                    ExtensionsMethods.SendKeysToElement(inputPassword, microvixUser.senha);
                    ExtensionsMethods.SendKeysToElement(buttonEnter, Keys.Enter);
                }
            }
            catch
            {
                throw;
            }
        }

        public void SelectCompany(string cnpj, MicrovixUser microvixUser, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var comboBoxSelectByText = string.Empty;
                var valueSelectByValue = string.Empty;

                if (cnpj == "42538267000268")
                {
                    comboBoxSelectByText = "5 - Open Era - Ecommerce";
                    valueSelectByValue = "5";
                }
                else if (cnpj == "38367316000199")
                {
                    comboBoxSelectByText = "1 - Misha - Ecommerce";
                    valueSelectByValue = "1";
                }
                else if (cnpj == "38367316000865")
                {
                    comboBoxSelectByText = "17 - MISHA - Volo";
                    valueSelectByValue = "14";
                }

                var comboBoxSelectCompany = ExtensionsMethods.GetElementExistsById("sel_empresa_portal_usuario", _wait, Page.TypeEnum.Login, "SelectCompany");
                var flagChoseCompany = ExtensionsMethods.ChecksIfElementIsVisibleById("quantidade_empresa", _wait, Page.TypeEnum.Login, "SelectCompany");
                var flagSelectTextIsPresent = ExtensionsMethods.ChecksIfTextIsPresentInComboBox(comboBoxSelectByText, _wait, Page.TypeEnum.Login, "SelectCompany", comboBoxSelectCompany);
                var flagTopbarSelCompany = ExtensionsMethods.ChecksIfElementExixtsById("topbar_sel_empresa_portal_usuario", _wait, Page.TypeEnum.Login, "SelectCompany");
                var buttonSelect = ExtensionsMethods.GetElementToBeClickableById("btnselecionar_empresa", _wait, Page.TypeEnum.Login, "SelectCompany");
                var comboBoxCompany = new SelectElement(comboBoxSelectCompany);

                _driver.SwitchTo().DefaultContent();

                if (flagChoseCompany)
                {
                    if (flagSelectTextIsPresent)
                    {
                        comboBoxCompany.SelectByText(comboBoxSelectByText);
                        ExtensionsMethods.ClickInElement(buttonSelect);
                    }
                }

                if (flagTopbarSelCompany)
                {
                    var dropDownMenu = ExtensionsMethods.GetElementExistsById("topbar_sel_empresa_portal_usuario", _wait, Page.TypeEnum.Login, "SelectCompany");
                    var selectDropDownMenu = new SelectElement(dropDownMenu);

                    selectDropDownMenu.SelectByValue(valueSelectByValue);
                }
            }
            catch
            { 
                throw;
            }
        }
    }
}
