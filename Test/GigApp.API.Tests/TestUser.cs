using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using GIgApp.Contracts.Responses;

namespace GigApp.API.Tests;

public class TestUser
{
    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public string Email { get; set; }
    public string Password { get; set; }
    public string firebase_token { get; set; } = "string";
    public string device_info { get; set; } = "string";
    protected string AuthToken { get; set; }
    public HttpClient Client { get; set; }

    

    public TestUser(string email, string password)
    {
        Email = email;
        Password = password;
        
        Client = new HttpClient();
        Client.BaseAddress = new Uri("https://gig-api-stage.azurewebsites.net"); // Replace with your API URL
    }

    public virtual async Task<bool> Login()
    {
        var response = await Client.PostAsJsonAsync("/api/login", new
        {
            Email,
            Password,
            firebase_token,
            device_info
        });
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseResult>();
            var loginResult = JsonSerializer.Deserialize<LoginResult>(result?.Data.ToString() ?? string.Empty, options ?? new JsonSerializerOptions());
            AuthToken = loginResult?.access_token ?? string.Empty ;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
            return true;
        }
        else{
            var result = await response.Content.ReadFromJsonAsync<BaseFailResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result?.Error}");
            return false;
        }
    }
}