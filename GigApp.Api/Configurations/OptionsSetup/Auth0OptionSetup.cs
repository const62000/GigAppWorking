using GigApp.Application.Options.Auth0;
using Microsoft.Extensions.Options;

namespace GigApp.Api.Configurations.OptionsSetup
{
    public class Auth0OptionSetup(IConfiguration _configuration) : IConfigureOptions<Auth0Option>
    {
        public void Configure(Auth0Option options)
        {
            _configuration.GetSection(options.SectionName).Bind(options);
        }
    }
}
