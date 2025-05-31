using GigApp.Api.Configurations.OptionsSetup;

namespace GigApp.Api.Services
{
    public static class OptionsRegisteration
    {
        public static IServiceCollection AddOption(this IServiceCollection services) 
        {
            services.ConfigureOptions<Auth0OptionSetup>();
            services.ConfigureOptions<DatabaseOptionSetup>();
            services.ConfigureOptions<MailServerOptionSetup>();
            return services;
        }
    }
}
