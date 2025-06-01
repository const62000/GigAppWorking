using GIgApp.Contracts.Requests.Login;
using GIgApp.Contracts.Requests.Signup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.Authorization;
using VMS.Client.Repositories.Abstractions;
using VMS.Client.Repositories.Implementations;
using VMS.Client.Handlers;

namespace VMS.Client.DependencyInjection;
public static class DI
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettings = "https://gigapp.runasp.net";
        services.AddScoped<AuthorizationMessageHandler>();

        services.AddHttpClient<IFacilitiesRepository, FacilitiesRepository>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(apiSettings); // Set the base URL from configuration
        }).AddHttpMessageHandler<AuthorizationMessageHandler>();
        // services.AddHttpClient<IFacilitiesRepository, FacilitiesRepository>((serviceProvider, client) =>
        // {
        //     client.BaseAddress = new Uri(apiSettings); // Set the base URL from configuration
        // }).AddHttpMessageHandler<AuthorizationMessageHandler>();
        services.AddHttpClient<IAuthRepository, AuthRepository>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(apiSettings); // Set the base URL from configuration
        }).AddHttpMessageHandler<AuthorizationMessageHandler>();
        services.AddHttpClient<IVendorRepository, VendorRepository>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(apiSettings); // Set the base URL from configuration
        }).AddHttpMessageHandler<AuthorizationMessageHandler>();
        services.AddHttpClient<ITimeSheetRepository, TimeSheetRepository>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(apiSettings); // Set the base URL from configuration
        }).AddHttpMessageHandler<AuthorizationMessageHandler>();
        services.AddHttpClient<IFreelancerRepository, FreelancerRepository>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(apiSettings); // Set the base URL from configuration
        }).AddHttpMessageHandler<AuthorizationMessageHandler>();
        return services;
    }
}