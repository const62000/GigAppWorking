using GigApp.Application.Options.MailServer;
using Microsoft.Extensions.Options;

namespace GigApp.Api.Configurations.OptionsSetup
{
    public class MailServerOptionSetup(IConfiguration _configuration) : IConfigureOptions<MailServerOption>
    {
        public void Configure(MailServerOption options)
        {
            _configuration.GetSection(options.SectionName).Bind(options);
        }
    }
}
