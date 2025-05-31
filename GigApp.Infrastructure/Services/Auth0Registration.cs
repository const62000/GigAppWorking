using GigApp.Application.Options.Auth0;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GigApp.Infrastructure.Services
{
    internal static class Auth0Registration
    {
        internal static IServiceCollection AddAuth0(this IServiceCollection services)
        {
            Auth0Option? auth0Options = services.BuildServiceProvider().GetRequiredService<IOptions<Auth0Option>>()!.Value;
            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(auth0Options.Domain)
            });
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (options) =>
                {
                    options.ClaimsIssuer = auth0Options.Domain + "/";
                    options.Audience = auth0Options.Audience;
                    options.Authority = auth0Options.Domain + "/";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                    };
                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnAuthenticationFailed = con =>
                    //    {
                    //        return Task.CompletedTask;
                    //    },
                    //    OnChallenge = context =>
                    //    {
                    //        var x = context.Error;
                    //        // Add custom logging or responses
                    //        return Task.CompletedTask;
                    //    },
                    //    OnForbidden = context =>
                    //    {
                    //        var x = context.Result;
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin"));
            });
            return services;
        }
    }
}
