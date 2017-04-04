using RSI.Web.Tests.PageLibrary;
using System;
using TechTalk.SpecFlow;

namespace RSI.Web.Tests
{
    public class StepBase : Steps
    {
        protected ITestFixture TestFixture
        {
            get { return _testFixture; }
        }

        private readonly ITestFixture _testFixture = new TestFixture();

        public ApplicationNavigation Navigation { get; private set; }

        protected IBasePageObject CurrentPage
        {
            get { return (IBasePageObject)ScenarioContext.Current["CurrentPage"]; }
            set { ScenarioContext.Current["CurrentPage"] = value; }
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            if (!ScenarioContext.Current.ContainsKey("CurrentDriver"))
            {
                TestFixture.Test_Setup();
                ScenarioContext.Current.Add("CurrentDriver", TestFixture.CurrentDriver);
                Navigation = new ApplicationNavigation(_testFixture.CurrentDriver);
                ScenarioContext.Current.Add("Navigation", Navigation);
            }
            else
            {
                Navigation = (ApplicationNavigation)ScenarioContext.Current["Navigation"];
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (ScenarioContext.Current.ContainsKey("CurrentDriver"))
            {
                TestFixture.Test_Teardown();
                ScenarioContext.Current.Remove("CurrentDriver");
            }
        }
    }
}
