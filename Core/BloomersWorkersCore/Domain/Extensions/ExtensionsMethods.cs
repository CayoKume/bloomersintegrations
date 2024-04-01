using OpenQA.Selenium;

namespace BloomersWorkersCore.Domain.Extensions
{
    public class ExtensionsMethods
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

        /*public static IWebElement WaitElementUntilIsVisibile(By locator)
        {
            try
            {
                if (PropertiesCollection._driver.FindElement(locator).Enabled)
                    return PropertiesCollection._driver.FindElement(locator);
                else
                    return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }*/

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

        /*public static bool WaitElementToBeClickable(By locator)
        {
            try
            {
                if (PropertiesCollection._driver.FindElement(locator).Enabled)
                    return true;
                else
                    return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }*/

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

        /*public static ReadOnlyCollection<IWebElement> VisibilityOfAllElementsLocatedBy(By locator)
        {
            try
            {
                ReadOnlyCollection<IWebElement> readOnlyCollection = PropertiesCollection._driver.FindElements(locator);
                if (readOnlyCollection.Any((element) => !element.Displayed))
                    return null;
                else
                    return readOnlyCollection;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }

        public static bool WaitTextToBePresentInElement(IWebElement element, string text)
        {
            try
            {
                Thread.Sleep(2 * 1000);
                if (element.Text.Contains(text))
                    return true;
                else
                    return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        public static IWebDriver FrameToBeAvailableAndSwitchToIt(By locator)
        {
            try
            {
                IWebElement frameElement = PropertiesCollection._driver.FindElement(locator);
                return PropertiesCollection._driver.SwitchTo().Frame(frameElement);
            }
            catch (NoSuchFrameException)
            {
                return null;
            }
        }*/
    }
}
