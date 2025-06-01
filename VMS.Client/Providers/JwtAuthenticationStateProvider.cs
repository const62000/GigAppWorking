using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace VMS.Client.Providers;
public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _storage;
    public JwtAuthenticationStateProvider(ILocalStorageService storage)
    {
        _storage = storage;
    }
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await _storage.ContainKeyAsync("access_token"))
        {
            var tokenAsString = await _storage.GetItemAsync<string>("access_token");
            if (string.IsNullOrEmpty(tokenAsString))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.ReadJwtToken(tokenAsString);
            var identity = new ClaimsIdentity(token.Claims, "Bearer");

            // Manually add role claims
            var roleClaims = token.Claims.Where(c => c.Type == "role");
            foreach (var roleClaim in roleClaims)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
            }

            // Debugging: Print out all claims
            Console.WriteLine(identity.Claims);
            foreach (var claim in identity.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
            return authState;
        }
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity()); // Empty claims and
                                                                       // authentication scheme provided
        var anonymousAuthState = new AuthenticationState(anonymousUser);
        NotifyAuthenticationStateChanged(
        Task.FromResult(anonymousAuthState));
        return anonymousAuthState;
    }

    public async Task NotifyUserLogout()
    {
        await _storage.RemoveItemAsync("access_token");
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var anonymousAuthState = new AuthenticationState(anonymousUser);
        NotifyAuthenticationStateChanged(Task.FromResult(anonymousAuthState));
    }
}
