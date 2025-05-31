using Bogus.DataSets;
using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Enums;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace GigApp.Infrastructure.Repositories.Users
{
    public class UserRepository(JsonSerializerOptions options, ApplicationDbContext _applicationDbContext, IFacilityManagerRepository _facilityManagerRepository, IFreelancerRepository _freelancerRepository) : IUserRepository
    {
        public async Task<BaseResult> AddUser(User user, List<UserType> userTypes)
        {
            try
            {
                await _applicationDbContext.AddAsync(user);
                await _applicationDbContext.SaveChangesAsync();
                foreach (var item in userTypes)
                {
                    if (item == UserType.Freelancer)
                        await _freelancerRepository.AddFreelancer(user.Id);
                    else if (item == UserType.FacilityManager)
                        await _facilityManagerRepository.AddFacilityManager(user.Id);
                }
            }
            catch (DbUpdateException ex)
            {
                return new BaseResult(user.Id, false, Messages.UserAlreadyExists);
            }
            catch (Exception ex)
            {
                return new BaseResult(user.Id, false, Messages.SignUp_Fail);
            }
            return new BaseResult(user.Id, true, string.Empty);
        }
        public async Task<long> GetUserId(string auth0Id)
        {
            var user = await _applicationDbContext.Users.FirstAsync(x => x.Auth0UserId == auth0Id);
            return user.Id;
        }
        public async Task<User> GetUserById(long id)
        {
            return await _applicationDbContext.Users.FirstAsync(x => x.Id == id);
        }
        public async Task<BaseResult> GetCurrentUser(string auth0Id)
        {
            var user = await _applicationDbContext.Users
                .IncludeUserDetails()
                .FirstOrDefaultAsync(x => x.Auth0UserId == auth0Id);



            if (user == null)
            {
                return new BaseResult(new { }, false, Messages.User_Not_Found); // Handle not found
            }
            //var userResponse = new CurrentUserResult
            //{
            //    FirstName = user.FirstName??string.Empty,
            //    LastName = user.LastName??string.Empty,
            //    Email = user.Email,
            //    ProfilePic = user.Picture??string.Empty,
            //    UserType =new List<string>() { "Job Provider"},
            //    Address = user.Addresses.Select(a => new AddressResult
            //    {
            //        StreetAddress1 = a.StreetAddress1??string.Empty,
            //        StreetAddress2 = a.StreetAddress2??string.Empty,
            //        City = a.City,
            //        StateProvince = a.StateProvince??string.Empty,
            //        PostalCode = a.PostalCode??string.Empty,
            //        Country = a.Country,
            //        Latitude = a.Latitude ?? 0,
            //        Longitude = a.Longitude ?? 0
            //    }).FirstOrDefault()?? new AddressResult(),
            //    Licenses = user.FreelancerLicenses.Select(l => new LicenseResult
            //    {
            //        LicenceName = l.LicenseName??string.Empty,
            //        DateOfIssue = l.IssuedDate.ToDateTime(new TimeOnly()), // Convert DateOnly to DateTime
            //        FileUrl = l.LicenseFileUrl??string.Empty,
            //        IssueBy = l.IssuedBy,
            //        LicenseNumber = l.LicenseNumber??string.Empty
            //    }).ToList()
            //};
            //if(await _applicationDbContext.Freelancers.AnyAsync(x => x.UserId == Convert.ToInt32(user.Id)))
            //    userResponse.UserType.Add("Freelancer");
            //if (await _applicationDbContext.FacilityManagers.AnyAsync(x => x.UserId == Convert.ToInt32(user.Id)))
            //    userResponse.UserType.Add("Facility Managers");


            // Serialize the user object with the configured options
            //var userJson = JsonSerializer.Serialize(user, options);

            return new BaseResult(user, true, string.Empty);
        }

        public async Task<BaseResult> UpdateUserProfile(string auth0Id, string picture)
        {
            var result = await _applicationDbContext.Users.Where(x => x.Auth0UserId == auth0Id).ExecuteUpdateAsync(s =>
            s.SetProperty(x => x.Picture, picture)
            );
            if (result > 0)
                return new BaseResult(await GetCurrentUser(auth0Id), true, string.Empty);
            return new BaseResult(new { }, false, string.Empty);
        }

        public async Task<BaseResult> AddAuth0Id(long id, string auth0Id)
        {
            var result = await _applicationDbContext.Users.Where(x => x.Id == id).ExecuteUpdateAsync(s =>
            s.SetProperty(x => x.Auth0UserId, auth0Id)
            );
            if (result > 0)
                return new BaseResult(new { }, true, string.Empty);
            return new BaseResult(new { }, false, string.Empty);
        }
        public async Task<BaseResult> UpdateUserStripeAccountId(long userId, string stripeAccountId)
        {
            var result = await _applicationDbContext.Users.Where(x => x.Id == userId).ExecuteUpdateAsync(s =>
            s.SetProperty(x => x.StripeAccountId, stripeAccountId)
            );
            if (result > 0)
                return new BaseResult(new { }, true, Messages.StripeAccountIdUpdated);
            return new BaseResult(new { }, false, Messages.StripeAccountIdUpdateFailed);
        }
        public async Task<BaseResult> DeleteUser(long userId)
        {
            var userVendors = await _applicationDbContext.UserVendors.Where(x => x.UserId == userId).ExecuteDeleteAsync();
            var result = await _applicationDbContext.Users.Where(x => x.Id == userId).ExecuteDeleteAsync();
            if (result > 0)
                return new BaseResult(new { }, true, string.Empty);
            return new BaseResult(new { }, false, string.Empty);
        }
    }
}
