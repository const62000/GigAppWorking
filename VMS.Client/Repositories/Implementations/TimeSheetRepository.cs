using VMS.Client.Repositories.Abstractions;
using GIgApp.Contracts.Requests.Vendor;
using GIgApp.Contracts.Responses;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using GIgApp.Contracts.Shared;
using GIgApp.Contracts.Requests.BankAccount;
using GigApp.Domain.Entities;
using VMS.Client.Providers;

namespace VMS.Client.Repositories.Implementations
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _authStateProvider;
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public TimeSheetRepository(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = (JwtAuthenticationStateProvider)authStateProvider;
        }

        public async Task<List<TimeSheet>> GetTimeSheetsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(EndPoints.TimeSheet_User);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                    if (data!.Status)
                    {
                        var timesheet = JsonSerializer.Deserialize<List<TimeSheet>>(data!.Data.ToString()!, options);
                        return timesheet!;
                    }
                    else
                    {
                        return new List<TimeSheet>();
                    }
                }
                else
                {
                    return new List<TimeSheet>();
                }
            }
            catch
            {
                return new List<TimeSheet>();
            }
        }

        public async Task<TimeSheet> GetTimeSheetByHiredIdAsync(int hiredId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<TimeSheet>>($"{EndPoints.TimeSheet_Hired}/{hiredId}");
                return response?.Data ?? new TimeSheet();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching timesheet: {ex.Message}");
                return new TimeSheet();
            }
        }

        public async Task<bool> ClockInAsync(TimeSheet timeSheet)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(EndPoints.ClockIn, timeSheet);
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clocking in: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ClockOutAsync(TimeSheet timeSheet)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(EndPoints.ClockOut, timeSheet);
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clocking out: {ex.Message}");
                return false;
            }
        }
    }
}