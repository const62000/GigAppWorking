using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace VMS.Client.UITests.PageObjects;

public class LoginPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public LoginPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    // Element locators
    private IWebElement EmailInput => _wait.Until(d => d.FindElement(By.Id("email")));
    private IWebElement PasswordInput => _wait.Until(d => d.FindElement(By.Id("password")));
    private IWebElement LoginButton => _wait.Until(d => d.FindElement(By.Id("login-button")));

    public void Navigate()
    {
        _driver.Navigate().GoToUrl($"{TestConfiguration.BaseUrl}/login");
        _wait.Until(d => IsOnPage());
    }

    public void Login(string email, string password)
    {
        EmailInput.Clear();
        EmailInput.SendKeys(email);
        PasswordInput.Clear();
        PasswordInput.SendKeys(password);
        LoginButton.Click();
    }

    public bool IsOnPage()
    {
        return _driver.Url.Contains("/login", StringComparison.OrdinalIgnoreCase);
    }

    public string GetPageTitle()
    {
        return _wait.Until(d => d.Title);
    }
}