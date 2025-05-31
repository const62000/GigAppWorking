using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Options.Auth0;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GigApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GigApp.Infrastructure.Database;


namespace GigApp.Infrastructure.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        private readonly Auth0Option _options;
        private readonly ApplicationDbContext _context;
        private readonly IManagementApiClient _managementApiClient;
        private readonly IAuthenticationApiClient _authenticationApiClient;
        public AuthRepository(HttpClient httpClient, IOptions<Auth0Option> options, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _context = context;
            _authenticationApiClient = new AuthenticationApiClient(_options.DomainName);
            _managementApiClient = new ManagementApiClient(_options.ManagementApiToken, _options.DomainName);
        }
        public async Task<BaseResult> Login(string username, string password)
        {
            var request = new ResourceOwnerTokenRequest
            {
                ClientId = _options.ClientId,
                ClientSecret = _options.ClientSecret,
                Scope = "openid profile email", // Scope can be adjusted depending on your requirements
                Username = username,
                Password = password,
                Audience = _options.Audience // The API audience, optional but recommended for securing APIs
            };
            try
            {
                // This will request the token from Auth0
                var tokenResponse = await _authenticationApiClient.GetTokenAsync(request);
                return new BaseResult(new LoginResult(tokenResponse.AccessToken, tokenResponse.ExpiresIn, tokenResponse.TokenType), true, string.Empty);
            }
            catch (Auth0.Core.Exceptions.ApiException authEx)
            {
                // Handle Auth0 specific authentication errors (invalid credentials, etc.)
                return new BaseResult(new { }, false, authEx.Message);
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP-related errors (e.g., network issues, DNS issues)
                return new BaseResult(new { }, false, httpEx.Message);
            }
            catch (TimeoutException timeoutEx)
            {
                // Handle timeout-related issues
                return new BaseResult(new { }, false, timeoutEx.Message);
            }
            catch (Exception ex)
            {
                // Handle the error (authentication failure, invalid credentials, etc.)
                return new BaseResult(new { }, false, ex.Message);
            }
        }

        public async Task<BaseResult> CreateRole(string name, string description)
        {
            try
            {
                var request = new RoleCreateRequest
                {
                    Name = name,
                    Description = description
                };

                var result = await _managementApiClient.Roles.CreateAsync(request);
                return new BaseResult(result, true, string.Empty);
            }
            catch (Auth0.Core.Exceptions.ApiException authEx)
            {
                return new BaseResult(new { }, false, $"Authentication failed: {authEx.Message}");
            }
        }

        public async Task<BaseResult> RegisterFreelancer(string email, string password)
        {
            throw new NotImplementedException();
        }

        // public async Task<BaseResult> AssignRoleInAuth0(string userId, string role)
        // {
        //     var assignRoleEndpoint = $"/api/v2/users/{userId}/roles";
        //     var content = new StringContent(JsonSerializer.Serialize(new
        //     {
        //         roles = new[] { role }
        //     }), System.Text.Encoding.UTF8, "application/json");
        //     var response = await _httpClient.PostAsync(assignRoleEndpoint, content);
        //     var data = await response.Content.ReadAsStringAsync();
        //     if (response.IsSuccessStatusCode)
        //     {
        //         return new BaseResult(new { }, true, "Role Assigned Successfully");
        //     }
        //     JsonDocument doc = JsonDocument.Parse(data);
        //     return ErrorMessage(response, doc);
        // }

        private static BaseResult ErrorMessage(HttpResponseMessage response, JsonDocument doc)
        {
            if (doc.RootElement.TryGetProperty("message", out JsonElement messageElement))
                return new BaseResult(new { }, false, $"Error Assigning Role ({response.StatusCode} - {response.ReasonPhrase}): {messageElement.GetString() ?? string.Empty}");
            if (doc.RootElement.TryGetProperty("description", out messageElement))
                return new BaseResult(new { }, false, $"Error Assigning Role ({response.StatusCode} - {response.ReasonPhrase}): {messageElement.GetString() ?? string.Empty}");
            if (doc.RootElement.TryGetProperty("error", out messageElement))
                return new BaseResult(new { }, false, $"Error Assigning Role ({response.StatusCode} - {response.ReasonPhrase}): {messageElement.GetString() ?? string.Empty}");
            return new BaseResult(new { }, false, $"Error Assigning Role ({response.StatusCode} - {response.ReasonPhrase}): Please try again later.");
        }

        // public async Task<BaseResult> RegisterUserInAuth0(string email, string password)
        // {
        //     var auth0Options = options.Value;
        //     var content = new StringContent(JsonSerializer.Serialize(new
        //     {
        //         client_id = auth0Options.ClientId,
        //         email = email,
        //         password = password,
        //         connection = auth0Options.Connection // Use your actual connection name here
        //     }), System.Text.Encoding.UTF8, "application/json");
        //     var response = await _httpClient.PostAsync(EndPoints.Auth0_signup_EndPoint, content);
        //     var data = await response.Content.ReadAsStringAsync();
        //     if (response.IsSuccessStatusCode)
        //     {
        //         var result = JsonSerializer.Deserialize<SignupResult>(data);
        //         if (result != null)
        //             return new BaseResult(result, true, string.Empty);
        //     }
        //     JsonDocument doc = JsonDocument.Parse(data);
        //     return ErrorMessage(response, doc);
        // }

        public async Task<BaseResult> SignUp(string email, string password)
        {
            try
            {
                var request = new UserCreateRequest
                {
                    Email = email,
                    Password = password,
                    Connection = _options.Connection,
                    EmailVerified = false
                };

                var result = await _managementApiClient.Users.CreateAsync(request);

                if (string.IsNullOrEmpty(result.UserId))
                {
                    return new BaseResult(result, false, "Error occurred with your email or password");
                }
                return new BaseResult(result, true, string.Empty);
            }
            catch (Auth0.Core.Exceptions.ApiException authEx)
            {
                // Handle Auth0 API exceptions (invalid credentials, etc.)
                return new BaseResult(new { }, false, $"Authentication failed: {authEx.Message}");
            }
            catch (Exception ex)
            {
                // General fallback for other exceptions
                return new BaseResult(new { }, false, ex.Message);
            }
        }

        public async Task<BaseResult> VendorLogin(string email, string password)
        {
            //var vendor = await _context.Vendors.FirstOrDefaultAsync(v => v.User.Email == email);
            // if (vendor == null)
            //     return new BaseResult(new { }, false, Messages.Vendor_Not_Found);
            try
            {
                var hasRole = await UserHasRoleByEmail(email, "Vendor") || await UserHasRoleByEmail(email, "Admin");
                if (!hasRole)
                    return new BaseResult(new { }, false, "You are not authorized to login as a vendor");
                return await Login(email, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking user role: {ex.Message}");
                return new BaseResult(new { }, false, ex.Message);
            }
        }

        public async Task<BaseResult> DeleteUser(string auth0UserId)
        {
            await _managementApiClient.Users.DeleteAsync(auth0UserId);
            return new BaseResult(new { }, true, "User deleted successfully");
        }
        public async Task<BaseResult> AssignRole(string auth0UserId, string roleName)
        {
            try
            {
                var roles = await _managementApiClient.Roles.GetAllAsync(new GetRolesRequest());
                var role = roles.FirstOrDefault(r => r.Name == roleName);
                if (role == null)
                    return new BaseResult(new { }, false, "Role Not Found");
                var request = new AssignRolesRequest
                {
                    Roles = new[] { role.Id }
                };
                await _managementApiClient.Users.AssignRolesAsync(auth0UserId, request);
                var userUpdateRequest = new UserUpdateRequest
                {
                    AppMetadata = new
                    {
                        role = roleName
                    }
                };
                await _managementApiClient.Users.UpdateAsync(auth0UserId, userUpdateRequest);
                return new BaseResult(new { }, true, "Registered Admin Successfully");
            }
            catch (Auth0.Core.Exceptions.ApiException authEx)
            {
                return new BaseResult(new { }, false, $"Authentication failed: {authEx.Message}");
            }
            catch (Exception ex)
            {
                return new BaseResult(new { }, false, ex.Message);
            }
        }

        public async Task<bool> UserHasRoleByEmail(string email, string roleName)
        {
            try
            {
                // Step 1: Retrieve the user by email
                var users = await _managementApiClient.Users.GetUsersByEmailAsync(email);
                var user = users.FirstOrDefault();
                if (user == null)
                {
                    return false; // User not found
                }

                // Step 2: Retrieve the user's roles
                return await UserHasRoleByAuth0Id(user.UserId, roleName);
            }
            catch (Auth0.Core.Exceptions.ApiException authEx)
            {
                Console.WriteLine($"Error checking user role: {authEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking user role: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UserHasRoleByAuth0Id(string auth0UserId, string roleName)
        {
            var roles = await _managementApiClient.Users.GetRolesAsync(auth0UserId);
            return roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
