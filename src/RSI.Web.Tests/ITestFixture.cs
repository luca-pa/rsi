using OpenQA.Selenium;

namespace RSI.Web.Tests
{
    public interface ITestFixture
    {
        void Test_Setup();
        void Test_Teardown();
        IWebDriver CurrentDriver { get; set; }
    }
}