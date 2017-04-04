using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using RSI.Web.Tests.PageLibrary;

namespace RSI.Web.Tests
{
    public class PortfolioPage : BasePageObject
    {
        public override string Url => "/portfolio";

        public int GetEtfsCount()
        {
            return Driver.FindElements(By.TagName("tr")).Count - 4;
        }

    }
}