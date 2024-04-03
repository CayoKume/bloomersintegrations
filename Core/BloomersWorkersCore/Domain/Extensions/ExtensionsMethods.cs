using BloomersWorkersCore.Domain.CustomExceptions;
using BloomersWorkersCore.Domain.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BloomersWorkersCore.Domain.Extensions
{
    public static class ExtensionsMethods
    {
        public static IWebElement GetElement(By locator, IWebDriver _driver)
        {
            try
            {
                return _driver.FindElement(locator);
            }
            catch
            {
                throw;
            }
        }

        public static IEnumerable<IWebElement> GetElements(By locator, IWebDriver _driver)
        {
            try
            {
                return _driver.FindElements(locator);
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public static bool ElementExist(By locator, IWebDriver _driver)
        {
            try
            {
                _driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static IWebElement ElementToBeClickable(IWebElement element, IWebDriver _driver)
        {
            try
            {
                if (element != null && element.Displayed && element.Enabled)
                    return element;
                else
                    return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public static bool ElementExistInAnotherWindow(By locator, string windowHandle, IWebDriver _driver)
        {
            try
            {
                _driver.SwitchTo().Window(windowHandle);
                _driver.FindElement(locator);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IWebElement GetElementInAnotherWindow(By locator, string parentWindowHandle, IWebDriver _driver)
        {
            try
            {
                _driver.SwitchTo().Window(parentWindowHandle);
                return _driver.FindElement(locator);
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public static IWebElement ElementToBeClickable(By locator, IWebDriver _driver)
        {
            try
            {
                var element = GetElement(locator, _driver);
                Thread.Sleep(2 * 1000);
                if (element.Enabled)
                    return element;
                else
                    return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public static bool ElementIsVisible(By locator, IWebDriver _driver)
        {
            try
            {
                var element = ElementIsVisible(_driver.FindElement(locator), _driver);
                if (element.Displayed && element.Enabled)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static IWebElement ElementIsVisible(IWebElement element, IWebDriver _driver)
        {
            Thread.Sleep(2 * 1000);
            if (!element.Displayed)
                return null;
            else
                return element;
        }
        /********************************************************************************************************************************************************************/
        public static void ClearInputElementValue(IWebElement element)
        {
            try
            {
                element.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception($" - {ex.Message}");
            }
        }

        public static void ClickInElement(IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch (Exception ex)
            {
                throw new Exception ($" - {ex.Message}");
            }
        }

        public static void SendKeysToElement(IWebElement element, string keys)
        {
            try
            {
                element.SendKeys(keys);
            }
            catch (Exception ex)
            {
                throw new Exception($" - {ex.Message}");
            }
        }

        public static bool ChecksIfElementExixtsById(string locator, WebDriverWait _wait, Page.TypeEnum page, string method)
        {
            try
            {
                var result = _wait.Until(ExpectedConditions.ElementExists(By.Id(locator)));

                if (result is not null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($@"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator} - {ex.InnerException.Message}");
            }
        }
        
        public static bool ChecksIfElementIsVisibleById(string locator, WebDriverWait _wait, Page.TypeEnum page, string method)
        {
            try
            {
                var result = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id(locator)));

                if (result is not null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator}",
                    locator,
                    Page.TypeEnum.Home
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator} - {ex.InnerException.Message}");
            }
        }

        public static bool ChecksIfTextIsPresentInComboBox(string locator, WebDriverWait _wait, Page.TypeEnum page, string method, IWebElement combobox)
        {
            try
            {
                return _wait.Until(ExpectedConditions.TextToBePresentInElement(combobox, locator));
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator}",
                    locator,
                    Page.TypeEnum.Home
                );
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static IWebElement GetElementToBeClickableById(string locator, WebDriverWait _wait, Page.TypeEnum page, string method)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(locator)));
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator}",
                    locator,
                    Page.TypeEnum.Home
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator} - {ex.InnerException.Message}");
            }
        }

        public static IWebElement GetElementToBeClickableByXpath(string locator, WebDriverWait _wait, Page.TypeEnum page, string method)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(locator)));
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator}",
                    locator,
                    Page.TypeEnum.Home
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator} - {ex.InnerException.Message}");
            }
        }

        public static IWebElement GetElementToBeClickableByClassName(string locator, WebDriverWait _wait, Page.TypeEnum page, string method)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName(locator)));
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator}",
                    locator,
                    Page.TypeEnum.Home
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator} - {ex.InnerException.Message}");
            }
        }

        public static IWebElement GetElementExixtsByXpath(string locator, WebDriverWait _wait, Page.TypeEnum page, string method)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementExists(By.XPath(locator)));
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator}",
                    locator,
                    Page.TypeEnum.Home
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator} - {ex.InnerException.Message}");
            }
        }

        public static IWebElement? GetElementExixtsById(string locator, WebDriverWait _wait, Page.TypeEnum page, string method)
        {
            try
            {
                var result = _wait.Until(ExpectedConditions.ElementExists(By.Id(locator)));

                if (result is not null)
                    return result;
                else
                    return null;
            }
            catch (Exception ex) when (ex.Message.Contains("Timed out"))
            {
                throw new CustomNoSuchElementException(
                    @$"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator}",
                    locator,
                    Page.TypeEnum.Home
                );
            }
            catch (Exception ex)
            {
                throw new Exception($@"{Enum.GetName(typeof(Page.TypeEnum), page)} ({method}) - O bot nao foi capaz de encontrar o elemento: {locator} - {ex.InnerException.Message}");
            }
        }
    }
}
