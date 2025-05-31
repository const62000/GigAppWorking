using GigApp.Application.Options.Database;
using GigApp.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GigApp.Infrastructure.Services
{
    internal static class DatabaseRegistration
    {
        internal static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddSingleton(new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            services.AddDbContextPool<ApplicationDbContext>((sp, options) =>
            {
                var databaseOptions = sp.GetService<IOptions<DatabaseOption>>()!.Value;
                options.UseSqlServer(databaseOptions.ConnectionString);
            });

            return services;
        }
    }
}
