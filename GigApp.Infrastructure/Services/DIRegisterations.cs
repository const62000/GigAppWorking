using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Interfaces.FileSaver;
using GigApp.Infrastructure.Repositories.Auth;
using LMS.Repositories.Implementation.FileSaver;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Infrastructure.Repositories.Jobs;

using Microsoft.Extensions.DependencyInjection;
using GigApp.Application.Interfaces.Users;
using GigApp.Infrastructure.Repositories.Users;
using GigApp.Application.Interfaces.Vendors;
using GigApp.Infrastructure.Repositories.Vendors;
using GigApp.Application.Interfaces.Notifications;
using GigApp.Infrastructure.Repositories.Notifications;
using GigApp.Infrastructure.Repositories.Facilities;
using GigApp.Application.Interfaces.Facilities;
using GigApp.Application.Interfaces.Addresses;
using GigApp.Infrastructure.Repositories.Addresses;
using GigApp.Application.Interfaces.Email;
using GigApp.Infrastructure.Repositories.Email;
using GigApp.Application.Interfaces.Payments;
using GigApp.Infrastructure.Repositories.Payments;
using GigApp.Application.Interfaces.Ratings;
using GigApp.Infrastructure.Repositories.Ratings;



namespace GigApp.Application.Services
{
    public static class DIRegisterations
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IFilesSaverRepository, FilesSaverRepository>();
            services.AddScoped<IJobRepository, JobsRepository>();
            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            services.AddScoped<IJobQuestionRepository, JobQuestionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IFreelancerRepository, FreelancerRepository>();
            services.AddScoped<IFacilityManagerRepository, FacilityManagerRepository>();
            services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IFacilitiesRepository, FacilitiesRepository>();
            services.AddScoped<IHiringRepository, HiringRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IMailSenderRepository, MailSenderRepository>();
            services.AddScoped<IAddressesRepository, AddressesRepository>();
            services.AddScoped<ITimeSheetRepository, TimeSheetRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IDisputeRepository, DisputeRepository>();
            services.AddScoped<IStripePaymentRepository, StripePaymentRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            return services;
        }
    }
}
