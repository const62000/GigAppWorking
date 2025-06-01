using GigApp.Domain.Entities;
using System.Net.Http.Json;
using GIgApp.Contracts.Shared;
using VMS.Client.Repositories.Abstractions;
using System.Text.Json;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Responses;
using VMS.Client.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using GIgApp.Contracts.Requests.Vendor;
namespace VMS.Client.Repositories.Implementations
{
    public class FacilitiesRepository : IFacilitiesRepository
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _authStateProvider;
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public FacilitiesRepository(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = (JwtAuthenticationStateProvider)authStateProvider;
        }

        public async Task<List<GigApp.Domain.Entities.Client>> GetClientsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(EndPoints.Facilities_VMS);
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new List<GigApp.Domain.Entities.Client>();
                }
                var result = JsonSerializer.Deserialize<BaseResult>(await response!.Content.ReadAsStringAsync(), options);
                if (result?.Status ?? false)
                {
                    var data = result?.Data!.ToString() ?? string.Empty;
                    return JsonSerializer.Deserialize<List<GigApp.Domain.Entities.Client>>(data, options) ?? new List<GigApp.Domain.Entities.Client>();
                }
                else
                    return new List<GigApp.Domain.Entities.Client>();
            }
            catch (Exception ex)
            {
                // Add proper logging here
                Console.WriteLine($"Error fetching clients: {ex.Message}");
                return new List<GigApp.Domain.Entities.Client>();
            }
        }

        public async Task<GigApp.Domain.Entities.Client> GetClientAsync(long id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{EndPoints.Facilities_EndPoint}/{id}");
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new GigApp.Domain.Entities.Client();
                }
                var result = JsonSerializer.Deserialize<ApiResponse<GigApp.Domain.Entities.Client>>(await response!.Content.ReadAsStringAsync(), options);
                return result?.Data ?? new GigApp.Domain.Entities.Client();
            }
            catch (Exception ex)


            {
                // Add proper logging here
                Console.WriteLine($"Error fetching client: {ex.Message}");
                return new GigApp.Domain.Entities.Client();
            }
        }

        public async Task<BaseResult> AddClientAsync(ClientRequest clientRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(EndPoints.Facilities_EndPoint, clientRequest);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                    return data!;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await _authStateProvider.NotifyUserLogout();
                        return new BaseResult(new { }, false, "Unauthorized");
                    }
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<BaseFailResult>(result, options);
                    return new BaseResult(data!.Data, false, data.Error);
                }

            }
            catch (Exception ex)
            {
                // Add proper logging here
                Console.WriteLine($"Error adding client: {ex.Message}");
                return new BaseResult(new { }, false, "Failed to add client");
            }
        }

        public async Task<BaseResult> EditFacility(ClientRequest clientRequest, long id)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{EndPoints.Facilities_EndPoint}/{id}", clientRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new BaseResult(new { }, false, "Unauthorized");
                }
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                    return data!;
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<BaseFailResult>(result, options);
                    return new BaseResult(data!.Data, false, data.Error);
                }
            }
            catch (Exception ex)
            {
                // Add proper logging here
                Console.WriteLine($"Error editing facility: {ex.Message}");
                return new BaseResult(new { }, false, "Failed to edit facility");
            }
        }


        public async Task<BaseResult> AddClientLocationAsync(ClientLocationRequest clientLocationRequest)

        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(EndPoints.Client_Location, clientLocationRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new BaseResult(new { }, false, "Unauthorized");
                }
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                    return data!;
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<BaseFailResult>(result, options);
                    return new BaseResult(data!.Data, false, data.Error);
                }
            }
            catch (Exception ex)
            {
                // Add proper logging here
                Console.WriteLine($"Error adding client: {ex.Message}");
                return new BaseResult(new { }, false, "Failed to add client");
            }
        }

        public async Task<bool> DeleteClientsAsync(DeleteClientRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{EndPoints.Facilities_EndPoint}/remove-multiple", request);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return false;
                }
                var result = await response.Content.ReadFromJsonAsync<BaseResult>();
                return result?.Status ?? false;
            }

            catch (Exception ex)
            {
                // Add proper logging here
                Console.WriteLine($"Error deleting facilities: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ClientLocation>> GetClientLocationsAsync(long clientId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{EndPoints.Facilities_EndPoint}/{clientId}/locations");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new List<ClientLocation>();
                }
                var baseresult = JsonSerializer.Deserialize<BaseResult>(await response.Content.ReadAsStringAsync(), options);
                if (baseresult?.Status ?? false)
                {
                    var data = JsonSerializer.Deserialize<List<ClientLocation>>(baseresult?.Data?.ToString() ?? string.Empty, options);
                    return data ?? new List<ClientLocation>();
                }
                else
                    return new List<ClientLocation>();
            }
            catch (Exception)
            {
                return new List<ClientLocation>();
            }
        }

        public async Task<List<User>> GetJobManagers(long clientId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{EndPoints.Facilities_EndPoint}/{clientId}/job-managers");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new List<User>();
                }
                var baseresult = JsonSerializer.Deserialize<BaseResult>(await response.Content.ReadAsStringAsync(), options);
                if (baseresult?.Status ?? false)
                {
                    var data = JsonSerializer.Deserialize<List<User>>(baseresult?.Data?.ToString() ?? string.Empty, options);
                    return data ?? new List<User>();
                }
                else
                    return new List<User>();


            }
            catch (Exception)
            {
                return new List<User>();
            }
        }

        public async Task<BaseResult> AddJobManager(AssignJobManagerRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{EndPoints.Facilities_EndPoint}/{request.ClientId}/job-managers", request);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authStateProvider.NotifyUserLogout();
                return new BaseResult(new { }, false, "Unauthorized");
            }
            return await response.Content.ReadFromJsonAsync<BaseResult>();
        }

        public async Task<BaseResult> DeleteJobManager(long clientId, long userId)
        {
            var response = await _httpClient.DeleteAsync($"{EndPoints.Facilities_EndPoint}/{clientId}/job-managers/{userId}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authStateProvider.NotifyUserLogout();
                return new BaseResult(new { }, false, "Unauthorized");
            }
            return await response.Content.ReadFromJsonAsync<BaseResult>();
        }
    }
    internal class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}