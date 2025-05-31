using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using VMS.Client.UITests.PageObjects;

namespace VMS.Client.UITests.Tests;

[TestClass]
public class LoginTests : TestBase
{
    private LoginPage _loginPage;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        _loginPage = new LoginPage(Driver);
    }

    [TestMethod]
    public void CanNavigateToLoginPage()
    {
        // Arrange & Act
        _loginPage.Navigate();

        // Assert
        Assert.IsTrue(_loginPage.IsOnPage());
        StringAssert.Contains(_loginPage.GetPageTitle(), "Login", StringComparison.OrdinalIgnoreCase);
    }

    [TestMethod]
    public void CanLoginWithValidCredentials()
    {
        // Arrange
        var (email, password) = TestConfiguration.TestUsers.DefaultUser;
        _loginPage.Navigate();

        // Act
        _loginPage.Login(email, password);

        // Assert
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.Url.Contains("/home", StringComparison.OrdinalIgnoreCase));
        StringAssert.Contains(Driver.Url.ToLower(), "/home");
    }

    [TestMethod]
    public void CannotLoginWithInvalidCredentials()
    {
        // Arrange
        _loginPage.Navigate();

        // Act
        _loginPage.Login("invalid@example.com", "wrongpassword");

        // Assert
        Assert.IsTrue(_loginPage.IsOnPage(), "Should remain on login page");
        // Add assertion for error message once implemented
    }

    [TestCleanup]
    public override void TestCleanup()
    {
        base.TestCleanup();
    }
}