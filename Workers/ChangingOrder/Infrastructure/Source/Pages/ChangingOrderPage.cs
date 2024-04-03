using BloomersWorkersCore.Domain.Enums;
using BloomersWorkersCore.Domain.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkers.ChangingOrder.Infrastructure.Source.Pages
{
    public class ChangingOrderPage : IChangingOrderPage
    {
        public bool AproveOrder(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var buttonOrderAproval = ExtensionsMethods.GetElementToBeClickableByXpath("/html/body/table[2]/tbody/tr[3]/td[2]/a", _wait, Page.TypeEnum.ChangingOrder, "AproveOrder");
                ExtensionsMethods.ClickInElement(buttonOrderAproval);

                var buttonFullAproval = ExtensionsMethods.GetElementToBeClickableById("b_at", _wait, Page.TypeEnum.ChangingOrder, "AproveOrder");
                ExtensionsMethods.ClickInElement(buttonFullAproval);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public void ProceedOrder(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var buttonAprove = ExtensionsMethods.GetElementToBeClickableById("B1", _wait, Page.TypeEnum.ChangingOrder, "AproveOrder");

                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", buttonAprove);

                ExtensionsMethods.ClickInElement(buttonAprove);
            }
            catch
            {
                throw;
            }
        }

        public void ChangeQtdeItemFromList(int cod_produto, int qtde_produto, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var tableItens = ExtensionsMethods.GetElementExistsById("tbl_itens", _wait, Page.TypeEnum.ChangingOrder, "ChangeQtdeItemFromList");
                var tableItensRows = ExtensionsMethods.GetElementsExistsByTagName("tr", Page.TypeEnum.ChangingOrder, "ChangeQtdeItemFromList", tableItens);

                for (int i = 1; i < tableItensRows.Count(); i++) //ignora thead
                {
                    var columnsRows = ExtensionsMethods.GetElementsExistsByTagName("td", Page.TypeEnum.ChangingOrder, "RemoveItemFromList", tableItensRows[i]);

                    if (columnsRows.First().Text.Contains($"{cod_produto}"))
                    {
                        var buttonEdit = ExtensionsMethods.GetElementsExistsByClassName("iconEdit", _wait, Page.TypeEnum.ChangingOrder, "RemoveItemFromList", tableItensRows[i]);
                        ExtensionsMethods.ClickInElement(buttonEdit.First());

                        var parentWindowHandle = _driver.CurrentWindowHandle;
                        var lstWindows = _driver.WindowHandles.ToList();
                        foreach (var window in lstWindows)
                        {
                            if (window != parentWindowHandle)
                            {
                                _driver.SwitchTo().Window(window);
                                _driver.Manage().Window.Maximize();
                            }
                            else
                                continue;
                        }

                        var inputQuantity = ExtensionsMethods.GetElementToBeClickableById("quantidade", _wait, Page.TypeEnum.ChangingOrder, "RemoveItemFromList");
                        var buttonAlter = ExtensionsMethods.GetElementToBeClickableByClassName("btn-submit", _wait, Page.TypeEnum.ChangingOrder, "RemoveItemFromList");

                        ExtensionsMethods.ClearInputElementValue(inputQuantity);
                        ExtensionsMethods.SendKeysToElement(inputQuantity, $"{qtde_produto}");
                        ExtensionsMethods.ClickInElement(buttonAlter);

                        _driver.SwitchTo().Window(parentWindowHandle);
                        _driver.Manage().Window.Maximize();

                        ExtensionsMethods.ChangeToFrameWhenItsAvaiable("main", _wait, Page.TypeEnum.ChangingOrder, "ChangeQtdeItemFromList");

                        Thread.Sleep(2 * 1000);
                        break;
                    }
                    else
                        continue;
                }
            }
            catch
            {
                throw;
            }
        }

        public void RemoveItemFromList(int cod_produto, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var tableItens = ExtensionsMethods.GetElementExistsById("tbl_itens", _wait, Page.TypeEnum.ChangingOrder, "RemoveItemFromList");
                var tableItensRows = ExtensionsMethods.GetElementsExistsByTagName("tr", Page.TypeEnum.ChangingOrder, "RemoveItemFromList", tableItens);
                
                for (int i = 1; i < tableItensRows.Count(); i++) //ignora thead
                {
                    var columnsRows = ExtensionsMethods.GetElementsExistsByTagName("td", Page.TypeEnum.ChangingOrder, "RemoveItemFromList", tableItensRows[i]);
                    
                    if (columnsRows.First().Text.Contains($"{cod_produto}"))
                    {
                        var buttonsDeleteItem = ExtensionsMethods.GetElementsExistsByClassName("lixeiraOn", _wait, Page.TypeEnum.ChangingOrder, "RemoveItemFromList", tableItensRows[i]);
                        ExtensionsMethods.ClickInElement(buttonsDeleteItem.First());
                        Thread.Sleep(2 * 1000);
                        break;
                    }
                    else
                        continue;
                }
            }
            catch
            {
                throw;
            }
        }

        public void SelectOrder(string nr_pedido, IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                Thread.Sleep(2 * 1000);

                ExtensionsMethods.ChangeToFrameWhenItsAvaiable("main", _wait, Page.TypeEnum.ChangingOrder, "SelectOrder");

                var inputBudget = ExtensionsMethods.GetElementToBeClickableById("orcamento", _wait, Page.TypeEnum.ChangingOrder, "SelectOrder");
                var buttonProceedFromSelectOrder = ExtensionsMethods.GetElementToBeClickableByName("B1", _wait, Page.TypeEnum.ChangingOrder, "SelectOrder");

                ExtensionsMethods.SendKeysToElement(inputBudget, $"{nr_pedido.Replace("MI-", "").Replace("OA-", "").Replace("VD", "").Replace("LJ", "")}");
                ExtensionsMethods.ClickInElement(buttonProceedFromSelectOrder);
            }
            catch
            {
                throw;
            }
        }

        public void ConfirmOrder(IWebDriver _driver, WebDriverWait _wait)
        {
            try
            {
                var buttonOkFromModal = ExtensionsMethods.GetElementToBeClickableByXpath("/html/body/div[4]/div[3]/div/button", _wait, Page.TypeEnum.ChangingOrder, "SelectOrder");
                var buttonProceedFromConfirmOrder = ExtensionsMethods.GetElementToBeClickableByName("B1", _wait, Page.TypeEnum.ChangingOrder, "SelectOrder");

                ExtensionsMethods.ClickInElement(buttonOkFromModal);
                ExtensionsMethods.ClickInElement(buttonProceedFromConfirmOrder);

                Thread.Sleep(2 * 1000);
            }
            catch
            {
                throw;
            }
        }
    }
}
