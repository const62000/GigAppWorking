using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Infrastructure.Repositories.Users
{
    internal class FreelancerRepository(ApplicationDbContext _applicationDbContext) : IFreelancerRepository
    {
        public async Task<BaseResult> GetFreelancers()
        {
            var freelancers = await _applicationDbContext.Freelancers.Select(x => x.UserId).ToListAsync();
            var users = await _applicationDbContext.Users.Where(x => freelancers.Contains(Convert.ToInt32(x.Id))).IncludeUserDetails().ToListAsync();
            return new BaseResult(users, true, string.Empty);
        }
        public async Task<BaseResult> AddFreelancer(long userId)
        {
            var freelancer = new Freelancer
            {
                UserId = Convert.ToInt32(userId),
            };
            await _applicationDbContext.Freelancers.AddAsync(freelancer);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, string.Empty);
        }

        public async Task<IEnumerable<FreelancerLicense>> GetFreelancerLicenseInfoAsync(long userId)
        {
            try
            {
                var licenseInfo = await _applicationDbContext.FreelancerLicenses.
                        Where(license => license.FreelancerUserId == userId)
                        .ToListAsync();

                if (licenseInfo == null || !licenseInfo.Any())
                {
                    return Enumerable.Empty<FreelancerLicense>();
                }

                return licenseInfo;
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                throw new Exception("An error occurred while fetching License details: " + ex.Message);
            }
        }
        public async Task<BaseResult> DisableFreelancer(long userId)
        {
            var result = await _applicationDbContext.Users.Where(x => x.Id == userId).ExecuteUpdateAsync(x => x.SetProperty(p => p.Disabled, true));
            return new BaseResult(new { }, true, string.Empty);
        }
        public async Task<BaseResult> EnableFreelancer(long userId)
        {
            var result = await _applicationDbContext.Users.Where(x => x.Id == userId).ExecuteUpdateAsync(x => x.SetProperty(p => p.Disabled, false));
            return new BaseResult(new { }, true, string.Empty);
        }
    }
}
