using OpenQA.Selenium;

namespace RSI.Web.Tests.PageLibrary
{
    public static class WebElementExtensions
    {
        /// <summary>
        /// With some headless browsers when butons are hidden inside a collapsable div, after 
        /// expanding that div, sometime the buton is not in the visible part of the page 
        /// so we use a trick to scroll down on the page and retry clicking on the button.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="driver"></param>
        public static void ClickAndRetryWithScroll(this IWebElement element, IWebDriver driver)
        {
            try
            {
                element.Click();
            }
            catch (ElementNotVisibleException)
            {
                // Scroll the page more and retry the click.
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(" + 500 + "," + 0 + ");");
                element.Click();
            }
        }
    }
}