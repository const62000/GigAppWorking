using System.Net.Http.Json;
using System.Text.Json;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;

namespace GigApp.API.Tests;

public class TestSuperAdmin : TestUser
{
    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public TestSuperAdmin() : base("akshjaaaiis@gmail.com", "P@ssword123") { }

    public async Task<long> CreateFacility(string name, AddressRequest address)
    {
        var response = await Client.PostAsJsonAsync<ClientRequest>("/api/facilities", 
            new ClientRequest(name , address)
        );
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseResult>();
            return 0;
        }
        else
        {
            var result = await response.Content.ReadFromJsonAsync<BaseFailResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result.Error}");
            response.EnsureSuccessStatusCode();
            return 0;
        }
    }

    public async Task<TestJobManager> CreateJobManager(SignupRequest signupRequest)
    {
        var response = await Client.PostAsJsonAsync("/api/signup", signupRequest);
        
        if(!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseFailResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result?.Error}");
            response.EnsureSuccessStatusCode();
        }
        return new TestJobManager(signupRequest.Email,signupRequest.Password);
    }

    public async Task<BaseResult> AssignFacilityToManager(string managerId, long facilityId)
    {
        var response = await Client.PostAsJsonAsync($"/api/facilities/{facilityId}/managers", new
        {
            ManagerId = managerId
        });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseResult>();
            //Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result.Message}");
            return result;
            //response.EnsureSuccessStatusCode();
        }
        else
        {
            var result = await response.Content.ReadFromJsonAsync<BaseResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result.Message}");
            //response.EnsureSuccessStatusCode();
            return result;
        }
    }
}