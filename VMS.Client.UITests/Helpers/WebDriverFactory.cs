using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace VMS.Client.UITests.Helpers;

public static class WebDriverFactory
{
    public static IWebDriver CreateDriver()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());

        var options = new ChromeOptions();
        // Add any needed Chrome options here
        // options.AddArgument("--headless"); // Uncomment to run tests without UI

        return new ChromeDriver(options);
    }
}