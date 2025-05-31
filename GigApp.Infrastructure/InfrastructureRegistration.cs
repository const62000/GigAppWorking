using GigApp.Application.Services;
using GigApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services) 
        {
            services.AddDI();
            services.AddAuth0();
            services.AddDatabase();
            services.AddFirebase();
            return services;
        }
    }
}
