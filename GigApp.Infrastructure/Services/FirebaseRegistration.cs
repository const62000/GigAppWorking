using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Infrastructure.Services
{
    public static class FirebaseRegistration
    {
        public static IServiceCollection AddFirebase(this IServiceCollection services)
        {
            var x = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gig-app-purescale-firebase-adminsdk-i20wj-3b0f1bd0cd.json"));

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gig-app-purescale-firebase-adminsdk-i20wj-3b0f1bd0cd.json"))
            });
            return services;
        }
    }
}
