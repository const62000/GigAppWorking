using GIgApp.Contracts.Responses;
using VMS.Client.Repositories.Abstractions;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using VMS.Client.Providers;
using GIgApp.Contracts.Shared;
using GigApp.Domain.Entities;

namespace VMS.Client.Repositories.Implementations;

public class FreelancerRepository : IFreelancerRepository
{
    private readonly HttpClient _httpClient;
    private readonly JwtAuthenticationStateProvider _authStateProvider;
    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public FreelancerRepository(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = (JwtAuthenticationStateProvider)authStateProvider;
    }

    public async Task<List<User>> GetFreelancers()
    {
        try
        {
            var response = await _httpClient.GetAsync(EndPoints.Freelancers);
            if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authStateProvider.NotifyUserLogout();
                return new List<User>();
            }
            var result = JsonSerializer.Deserialize<BaseResult>(await response!.Content.ReadAsStringAsync(), options);
            if (result?.Status ?? false)
            {
                var data = result?.Data!.ToString() ?? string.Empty;
                return JsonSerializer.Deserialize<List<User>>(data, options) ?? new List<User>();
            }
            else
                return new List<User>();
        }
        catch (Exception ex)
        {
            // Add proper loggin    g here
            Console.WriteLine($"Error fetching clients: {ex.Message}");
            return new List<User>();
        }
    }
    public async Task<BaseResult> ChangeFreelancerStatus(long userId, bool disabled)
    {
        try
        {
            var response = await _httpClient.GetAsync(EndPoints.Freelancers + $"/status/{userId}/{disabled}");
            return JsonSerializer.Deserialize<BaseResult>(await response!.Content.ReadAsStringAsync(), options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error changing freelancer status: {ex.Message}");
            return new BaseResult(new { }, false, ex.Message);
        }
    }
}

