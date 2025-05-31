namespace VMS.Client.UITests;

public static class TestConfiguration
{
    public static string BaseUrl => Environment.GetEnvironmentVariable("TEST_BASE_URL") ?? "https://localhost:7199";

    public static class TestUsers
    {
        public static (string Email, string Password) DefaultUser =>
            (Environment.GetEnvironmentVariable("TEST_USER_EMAIL") ?? "test@example.com",
             Environment.GetEnvironmentVariable("TEST_USER_PASSWORD") ?? "password123");
    }
}