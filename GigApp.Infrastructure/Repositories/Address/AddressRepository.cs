using GigApp.Application.Interfaces.Addresses;
using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;

namespace GigApp.Infrastructure.Repositories.Addresses
{
    public class AddressesRepository(ApplicationDbContext _context) : IAddressesRepository
    {
        public async Task<BaseResult> GetAddressAsync(long userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .IncludeAddressDetails()
                .ToListAsync();

            if (addresses == null)
            {
                throw new ArgumentException($"Address with ID {userId} not found.");
            }

            return new BaseResult(addresses, true, string.Empty);
        }
        public async Task<BaseResult> AddAddressAsync(string auth0Id, Address address)
        {
            var user = await _context.Users.Where(x => x.Auth0UserId == auth0Id).FirstOrDefaultAsync();
            if (user is null)
            {
                return new BaseResult(new { }, false, Messages.User_Not_Found);
            }
            address.UserId = user.Id;
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return new BaseResult(new { }, true, Messages.Address_Created);
        }
    }
}
