using GigApp.Application.Interfaces.Facilities;
using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using GigApp.Application.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Infrastructure.Repositories.Facilities
{
    internal class FacilitiesRepository(ApplicationDbContext _applicationDbContext, IUserRepository _userRepository, IAuthRepository _authRepository) : IFacilitiesRepository
    {
        public async Task<BaseResult> AddFacility(Client client, string auth0Id)
        {
            if (!string.IsNullOrEmpty(auth0Id))
                client.AdminUserId = await _userRepository.GetUserId(auth0Id);
            await _applicationDbContext.Clients.AddAsync(client);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, Messages.Facility_Created);
        }
        public async Task<BaseResult> EditFacility(Client client, string auth0Id)
        {
            if (!string.IsNullOrEmpty(auth0Id))
                client.AdminUserId = await _userRepository.GetUserId(auth0Id);
            _applicationDbContext.Clients.Update(client);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, "Facility updated successfully");
        }



        public async Task<BaseResult> DeleteFacilities(List<long> ids)
        {
            var result = await _applicationDbContext.Clients.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
            if (result > 0)
                return new BaseResult(new { }, true, Messages.Facility_Deleted);
            return new BaseResult(new { ids }, false, Messages.Facility_Deleted_Fail);
        }



        public async Task<BaseResult> GetFacilities()
        {
            var clients = await _applicationDbContext.Clients.IncludeClientDetails().ToListAsync();
            return new BaseResult(clients, true, string.Empty);
        }

        public async Task<BaseResult> GetClientLocationsAsync(long clientId)
        {
            var clientLocations = await _applicationDbContext.ClientLocations.Where(x => x.ClientId == clientId).IncludeClientLocationDetails().ToListAsync();
            return new BaseResult(clientLocations, true, string.Empty);
        }

        public async Task<BaseResult> GetFacilitiesAssociatedWithVendorManager(string auth0Id)
        {
            var clients = await _applicationDbContext.Clients.IncludeClientDetails().ToListAsync();
            return new BaseResult(clients, true, string.Empty);
        }
        public async Task<BaseResult> GetFacilities(string auth0Id, bool isAdmin)
        {
            if (!isAdmin)
            {
                if (!await _applicationDbContext.Clients.AnyAsync(x => x.AdminUser!.Auth0UserId == auth0Id))
                    return new BaseResult(new { }, false, "You are not a facility manager");
                var clients = await _applicationDbContext.Clients.Where(x => x.AdminUser!.Auth0UserId == auth0Id).IncludeClientDetails().ToListAsync();
                return new BaseResult(clients, true, string.Empty);
            }
            else
            {
                var clients = await _applicationDbContext.Clients.IncludeClientDetails().ToListAsync();
                return new BaseResult(clients, true, string.Empty);
            }
        }

        public async Task<BaseResult> GetFacility(long id)
        {
            var client = await _applicationDbContext.Clients.IncludeClientDetails().FirstOrDefaultAsync(x => x.Id == id) ?? new Client();
            return new BaseResult(client, true, string.Empty);
        }
        public async Task<BaseResult> AddClientLocation(ClientLocation clientLocation, string auth0Id)
        {
            if (!string.IsNullOrEmpty(auth0Id))
                clientLocation.CreatedByUserId = await _userRepository.GetUserId(auth0Id);
            await _applicationDbContext.ClientLocations.AddAsync(clientLocation);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, "Client location added successfully");
        }

        public async Task<BaseResult> GetJobManagers(long clientId)
        {
            var jobManagers = await _applicationDbContext.ClientManagers.Where(x => x.ClientId == clientId).Select(x => x.User).ToListAsync();
            return new BaseResult(jobManagers, true, string.Empty);
        }

        public async Task<BaseResult> AddJobManager(long clientId, string email)
        {
            var user = await _applicationDbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null)
                return new BaseResult(new { }, false, Messages.User_Not_Found);
            await _applicationDbContext.ClientManagers.AddAsync(new ClientManager { ClientId = clientId, UserId = user.Id });
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, "Job manager added successfully");
        }

        public async Task<BaseResult> DeleteJobManager(long clientId, long userId)
        {
            var result = await _applicationDbContext.ClientManagers.Where(x => x.ClientId == clientId && x.UserId == userId).ExecuteDeleteAsync();
            if (result > 0)
            {
                var user = await _userRepository.GetUserById(userId);
                await _authRepository.DeleteUser($"auth0|{user.Auth0UserId}");
                await _userRepository.DeleteUser(userId);
                return new BaseResult(new { }, true, "Job manager deleted successfully");
            }
            return new BaseResult(new { }, false, "Job manager not found");
        }
    }
}
