using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace RSI.Web.Tests.PageLibrary
{
    /// <summary>
    /// I do an application navigation object because that a PageObject is not supposed to be a "web page" 
    /// but more a part of a page, like a panel as said Martin Fowler (see http://martinfowler.com/bliki/PageObject.html#footnote-panel-object).
    /// So since a pageObject is not a page, it does't seem to be their responsability to navigate 
    /// from one application's screen to another. I prefer to create an application navigation object.
    /// </summary>
    /// <remarks>
    /// todo : see what think more advance experimented pageObject developper / users.
    /// </remarks>
    public class ApplicationNavigation
    {
        private readonly IWebDriver _driver;
        
        public ApplicationNavigation(IWebDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException("driver");            
        }

        public TPage FollowRedirect<TPage>() where TPage : BasePageObject, new()
        {
            WaitUntilDocumentIsReady();
            return BasePageObject.AttachInstance<TPage>(_driver);
        }

        public TPage NavigateTo<TPage>(int timeout = 0) where TPage : BasePageObject, new()
        {
            var pageInstance = BasePageObject.CreateInstance<TPage>();
            if (pageInstance.Url.ToLowerInvariant().StartsWith("http"))
            {
                var url = pageInstance.Url.TrimEnd('/') + pageInstance.QueryString;
                _driver.Navigate().GoToUrl(url);
            }
            else
            {
                var url = $"{pageInstance.BaseUrl.TrimEnd('/')}{GetAdaptedToEnvironmentUrl(pageInstance.Url)}{pageInstance.QueryString}";

                _driver.Navigate().GoToUrl(url);
            }

            if (timeout > 0)
                Thread.Sleep(timeout * 1000);

            WaitUntilDocumentIsReady();
            var page = BasePageObject.GetInstance<TPage>(_driver, pageInstance.DefaultTitle);

            return page;
        }

       
        protected virtual string GetAdaptedToEnvironmentUrl(string url)
        {
            string adaptedUrl = url;
            //if (Settings.CurrentSettings.TestExecutionEnvironment == "INT")
            //{//the real INT environment
            //    adaptedUrl = url.ToLowerInvariant().Replace("ncol/test", "ncol/int");
            //}
            //if (Settings.CurrentSettings.TestExecutionEnvironment == "INTONDEV")
            //{// the INT version deployed on DEV server environment
            //    adaptedUrl = url.ToLowerInvariant().Replace("ncol/test", "release/int");
            //}
            //if (Settings.CurrentSettings.TestExecutionEnvironment == "PROD")
            //{
            //    adaptedUrl = url.ToLowerInvariant().Replace("ncol/test", "ncol/prod");
            //}

            return adaptedUrl;
        }

        public void NavigateTo(string url, string queryString = "")
        {
            if (url.ToLowerInvariant().StartsWith("http"))
            {
                _driver.Navigate().GoToUrl(url.TrimEnd('/') + queryString);
            }
            else
            {
                throw new NotSupportedException();
            }
            WaitUntilDocumentIsReady();
        }

        public void NavigateToStaticUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
            WaitUntilDocumentIsReady();
        }

        public bool IsCurrentLocation<TPage>() where TPage : BasePageObject, new()
        {
            var pageInstance = BasePageObject.CreateInstance<TPage>();

            return (_driver.Url.ToLowerInvariant() == pageInstance.Url.ToLowerInvariant() || _driver.Url.ToLowerInvariant().Contains(pageInstance.Url.ToLowerInvariant()));
        }

        public void AssertIsCurrentLocation<TPage>() where TPage : BasePageObject, new()
        {
            if (!IsCurrentLocation<TPage>())
            {
                var pageInstance = BasePageObject.CreateInstance<TPage>();
                throw new AssertPageUrlException(pageInstance.Url, _driver.Url);
            }
        }

        private void WaitUntilDocumentIsReady()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(30);
            //var javaScriptExecutor = _driver.GetJavaScriptExecutor();
            //var wait = new WebDriverWait(_driver, timeout);

            //// Check if document is ready
            //Func<IWebDriver, bool> readyCondition = webDriver => (bool)javaScriptExecutor
            //    .ExecuteScript("return (document.readyState == 'complete' && " +
            //                            "(window.jQuery == undefined || (window.jQuery != undefined && jQuery.active == 0))"
            //    + ")");
            //wait.Until(readyCondition);
        }
    }
}
