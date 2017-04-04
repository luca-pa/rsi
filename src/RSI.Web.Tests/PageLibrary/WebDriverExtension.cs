using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace RSI.Web.Tests.PageLibrary
{
    public static class WebDriverExtension
    {
        public static IJavaScriptExecutor GetJavaScriptExecutor(this IWebDriver driver)
        {
            var result = driver as IJavaScriptExecutor;
            if (result == null)
                throw new NotSupportedException("This driver is not supporting javascript.");

            return result;
        }

        public static void SendKeysThroughJavascript(this IWebDriver webDriver, string controlId, string valueToSend)
        {

            GetJavaScriptExecutor(webDriver).ExecuteScript(string.Format("document.getElementById('{0}').value = '{1}';", controlId, valueToSend));
        }

        public static void SelectOptionThroughJavascript(this IWebDriver webDriver, string selectId, string valueToSelect)
        {
            GetJavaScriptExecutor(webDriver).ExecuteScript(string.Format("$(\"#{0}>option[value='{1}']\").prop('selected',true);", selectId, valueToSelect));
        }

        public static void ExecuteJavascript(this IWebDriver webDriver,  string snippet)
        {
            GetJavaScriptExecutor(webDriver).ExecuteScript(snippet);
        }

        
        public static IWebElement FindElementForAsynchronousViews(this IWebDriver webDriver, By by)
        {
            IWebElement element = null;
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(100));
            wait.Until(driver =>
            {
                var e = driver.FindElements(by);
                if (e.Count == 1)
                {
                    element = e.Single();
                }
                else if (e.Count > 1)
                {
                    throw new IndexOutOfRangeException("More than one element found.");
                }

                return element != null;
            });

            return element;
        }

    }
}