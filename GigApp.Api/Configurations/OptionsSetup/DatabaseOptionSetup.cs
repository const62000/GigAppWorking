using GigApp.Application.Options.Database;
using Microsoft.Extensions.Options;

namespace GigApp.Api.Configurations.OptionsSetup
{
    public class DatabaseOptionSetup(IConfiguration _configuration) : IConfigureOptions<DatabaseOption>
    {
        public void Configure(DatabaseOption options)
        {
            _configuration.GetSection(options.SectionName).Bind(options);

        }
    }
}
