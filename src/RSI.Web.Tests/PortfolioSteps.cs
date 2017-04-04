using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSI.Web.Tests.PageLibrary;
using System;
using TechTalk.SpecFlow;

namespace RSI.Web.Tests
{
    [Binding]
    public class PortfolioSteps : StepBase
    {
        [Given(@"I navigate to the portfolio page")]
        public void GivenINavigateToThePortfolioPage()
        {
            CurrentPage = Navigation.NavigateTo<PortfolioPage>(5);
        }

        [Then(@"there are (.*) etfs in the table")]
        public void ThenThereAreEtfsInTheTable(int etfs)
        {
            var page = CurrentPage.As<PortfolioPage>();
            Assert.AreEqual(etfs, page.GetEtfsCount());
        }
    }
}
