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


namespace VMS.Client.Repositories.Implementations;

public class VendorRepository : IVendorRepository
{
    private readonly HttpClient _httpClient;
    private readonly JwtAuthenticationStateProvider _authStateProvider;

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public VendorRepository(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = (JwtAuthenticationStateProvider)authStateProvider;
    }

    public async Task<BaseResult> AddVendor(VendorRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(EndPoints.Vendor_EndPoint, request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                return data!;
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
            return new BaseResult(new { }, false, ex.Message);
        }
    }
    public async Task<BaseResult> UpdateVendor(VendorRequest request, int vendorId)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(EndPoints.Vendor_EndPoint + $"/{vendorId}", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                return data!;
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
            return new BaseResult(new { }, false, ex.Message);
        }
    }
    public async Task<BaseResult> AddStaff(string email, int vendorId = 0)
    {
        try
        {
            var request = new StaffRequest(email, vendorId);
            var response = await _httpClient.PostAsJsonAsync(EndPoints.Staff_EndPoint, request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                return data!;
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
            return new BaseResult(new { }, false, ex.Message);
        }
    }
    public async Task<BaseResult> AddBankAccount(BankAccountRequet request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(EndPoints.BackAccount, request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                return data!;
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
            return new BaseResult(new { }, false, ex.Message);
        }
    }
    public async Task<List<User>> GetStaff()
    {
        try
        {
            var response = await _httpClient.GetAsync(EndPoints.Staff_EndPoint);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                if (data!.Status)
                {
                    var staff = JsonSerializer.Deserialize<List<User>>(data!.Data.ToString()!, options);
                    return staff!;
                }
                else
                {
                    return new List<User>();
                }
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new List<User>();
                }
                return new List<User>();
            }

        }
        catch
        {
            return new List<User>();
        }
    }
    public async Task<BaseResult> DeleteStaff(long userId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(EndPoints.Users + $"/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                return data!;
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new BaseResult(new { }, false, "Unauthorized");
                }
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseFailResult>(result, options);
                return new BaseResult(data!.Data, false, data.Error);
            }

        }
        catch
        {
            return new BaseResult(new { }, false, "Failed to delete staff");
        }
    }
    public async Task<List<Vendor>> GetVendors()
    {
        try
        {
            var response = await _httpClient.GetAsync(EndPoints.Vendor_EndPoint);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                if (data!.Status)
                {
                    var vendors = JsonSerializer.Deserialize<List<Vendor>>(data.Data.ToString()!, options);
                    return vendors!;
                }
                else
                {
                    return new List<Vendor>();
                }
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new List<Vendor>();
                }
                return new List<Vendor>();
            }
        }
        catch
        {
            return new List<Vendor>();
        }
    }
    public async Task<Vendor> GetVendorById(int vendorId)
    {
        try
        {
            var response = await _httpClient.GetAsync(EndPoints.Vendor_EndPoint + $"/{vendorId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                var vendor = JsonSerializer.Deserialize<Vendor>(data!.Data.ToString()!, options);
                return vendor!;
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.NotifyUserLogout();
                    return new Vendor();
                }
                return new Vendor();
            }

        }
        catch
        {
            return new Vendor();
        }
    }
    public async Task<BaseResult> AddVendorManager(AssignVendorManagerRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(EndPoints.VendorManager_EndPoint, request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<BaseResult>(result, options);
                return data!;
            }
            else
            {
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
            return new BaseResult(new { }, false, ex.Message);
        }

    }
    public async Task<BaseResult> DeleteVendors(DeleteVendorsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(EndPoints.Vendor_EndPoint + "/remove", request);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<BaseResult>(result, options);
            return data!;
        }
        else
        {
            if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authStateProvider.NotifyUserLogout();
                return new BaseResult(new { }, false, "Unauthorized");
            }
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<BaseFailResult>(result, options);
            return new BaseResult(data!.Data, false, data.Error);
        }


    }
}