using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace RSI.Web.Tests.PageLibrary
{
    public abstract class BasePageObject : IBasePageObject
    {
        public static IWebDriver Driver { get; set; }
        public virtual string BaseUrl => "http://localhost:81";
        public abstract string Url { get; }
        public virtual string DefaultTitle => "RSI";
        public virtual string QueryString { get; protected set; }
        protected bool UseUrlToValidatePage { get; set; }
        protected bool UseTitleToValidatePage { get; set; }

        public Func<bool> CustomPageValidation { get; set; }

        protected BasePageObject()
        {
            if (ScenarioContext.Current != null)
            {
                Driver = ScenarioContext.Current.Get<IWebDriver>("CurrentDriver");

                //UseUrlToValidatePage = FeatureContext.Current.IsUseUrlToValidatePage();
                //UseTitleToValidatePage = FeatureContext.Current.IsUseTitleToValidatePage();
            }
        }

        public static TPage GetInstance<TPage>(IWebDriver driver, string expectedTitle = "")
            where TPage : BasePageObject, new()
        {
            var pageInstance = CreateInstance<TPage>();
            PageFactory.InitElements(driver, pageInstance);

            if (string.IsNullOrWhiteSpace(expectedTitle))
            {
                expectedTitle = pageInstance.DefaultTitle;
            }

            // Wait up to 5s for an actual page title since Selenium no longer waits for page to load after 2.21
            new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                .Until(d => d.FindElement(By.TagName("body")));

            Assert.AreEqual(expectedTitle, driver.Title, "Page Title");

            return pageInstance;
        }

        public static TPage AttachInstance<TPage>(IWebDriver driver) where TPage : BasePageObject, new()
        {
            var pageInstance = CreateInstance<TPage>();
            PageFactory.InitElements(driver, pageInstance);

            return pageInstance;
        }

        public static TPage CreateInstance<TPage>() where TPage : BasePageObject, new() => new TPage();
        public static string GetLocation(IWebDriver driver) => driver.Url;

        /// <summary>
        /// Asserts that the current page is of the given type
        /// </summary>
        public void AssertIs<TPage>() where TPage : BasePageObject, new()
        {
            var page = this as TPage;

            if (page == null)
            {
                throw new AssertionExceptionBase($"Page Type Mismatch: Current page is not a '{typeof(TPage).Name}'");
            }

            Assert.IsTrue(page.ValidatePage());
        }

        protected virtual bool ValidatePage()
        {
            if (CustomPageValidation != null)
            {
                return CustomPageValidation();
            }

            if (UseTitleToValidatePage)
            {
                Assert.AreEqual(DefaultTitle, Driver.Title, "Page title mismatch");
            }
            if (UseUrlToValidatePage)
            {
                Assert.IsTrue(Driver.Url.ToLowerInvariant().Contains(Url.ToLowerInvariant()), $"Page url mismatch - actual <{Driver.Url}> does not contain expected <{Url}>'");
            }
            return true;
        }

        protected void WaitUntil(Func<bool> condition, int milliseconds = 100)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(milliseconds * 1000));
            wait.Until(drv => condition());
        }
    }
}
