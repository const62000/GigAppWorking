using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using VMS.Client.UITests.Helpers;

namespace VMS.Client.UITests.Tests;

public class TestBase
{
    protected IWebDriver Driver { get; private set; }
    protected const string BaseUrl = "https://localhost:5001";

    [TestInitialize]
    public virtual void TestInitialize()
    {
        Driver = WebDriverFactory.CreateDriver();
    }

    [TestCleanup]
    public virtual void TestCleanup()
    {
        Driver?.Quit();
        Driver?.Dispose();
    }
}