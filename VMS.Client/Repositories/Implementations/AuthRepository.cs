using System.Text.Json;
using System.Net.Http.Json;
using VMS.Client.Repositories.Abstractions;
using GIgApp.Contracts.Requests.Login;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using GIgApp.Contracts.Requests.Signup;
using GigApp.Domain.Entities;
using VMS.Client.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace VMS.Client.Repositories.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly HttpClient _httpClient;
    private readonly JwtAuthenticationStateProvider _authStateProvider;
    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public AuthRepository(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = (JwtAuthenticationStateProvider)authStateProvider;
    }
    public async Task<(string, string)> Login(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(EndPoints.Login, request);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                var loginResult = JsonSerializer.Deserialize<LoginResult>(data!.Data.ToString() ?? string.Empty, options ?? new JsonSerializerOptions());
                return (loginResult!.access_token, data.Message ?? string.Empty);
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return (null!, "Unauthorized");
                }
                var data = JsonSerializer.Deserialize<BaseFailResult>(result, options);
                return (null!, data!.Error ?? string.Empty);
            }

        }
        catch (Exception ex)
        {
            return (string.Empty, ex.Message ?? string.Empty);
        }
    }
    public async Task<(SignupResult, string)> Signup(SignupRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(EndPoints.SignUp, request);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return (null!, "Unauthorized");
                }
                var data = JsonSerializer.Deserialize<BaseFailResult>(result, options);
                var data2 = JsonSerializer.Deserialize<BaseResult>(data!.Data.ToString() ?? string.Empty, options);
                return (null!, data2!.Message ?? string.Empty);
            }

            else
            {
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                return (JsonSerializer.Deserialize<SignupResult>(data!.Data.ToString() ?? string.Empty, options ?? new JsonSerializerOptions()) ?? new SignupResult(string.Empty, string.Empty, string.Empty, false), data.Message ?? string.Empty);
            }

        }
        catch (Exception ex)
        {
            return (new SignupResult(string.Empty, string.Empty, string.Empty, false), ex.Message ?? string.Empty);
        }
    }
    public async Task<User?> GetCurrentUser()
    {
        try
        {
            var response = await _httpClient.GetAsync(EndPoints.CurrentUser);
            var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authStateProvider.NotifyUserLogout();
                return null;
            }
            if (response.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                var user = JsonSerializer.Deserialize<User>(data!.Data.ToString() ?? string.Empty, options ?? new JsonSerializerOptions());
                return user!;
            }
            else
            {
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                return null;
            }
        }
        catch (Exception)
        {
            await _authStateProvider.NotifyUserLogout();
            return null;
        }
    }
}