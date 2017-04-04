using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace RSI.Web.Tests
{
    public class TestFixture : ITestFixture
    {
        const string FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox";
        private IWebDriver _currentDriver;

        public IWebDriver CurrentDriver
        {
            get { return _currentDriver; }
            set { _currentDriver = value; }
        }

        public void Test_Setup()
        {
            _currentDriver = new FirefoxDriver();
            //_currentDriver.Manage().Window.Maximize();
        }

        public void Test_Teardown()
        {
            _currentDriver.Quit();
            _currentDriver.Dispose();
        }
    }
}
