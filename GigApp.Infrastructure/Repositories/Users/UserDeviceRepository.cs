using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;


namespace GigApp.Infrastructure.Repositories.Users
{
    internal class UserDeviceRepository(ApplicationDbContext _applicationDbContext) : IUserDeviceRepository
    {
        public async Task<BaseResult> AddUserDevice(UserDevice device,string email)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if(user == null)
                return new BaseResult(new { }, true, string.Empty);
            device.UserId = user.Id;
            if(await _applicationDbContext.UserDevices.AnyAsync(x=>x.UserId == device.UserId))
            {
                var result = await UpdateUserDevice(device);
                if(result.Status)
                    return new BaseResult(new { }, true, string.Empty);
            }
            await _applicationDbContext.UserDevices.AddAsync(device);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new {},true,string.Empty);
        }

        public async Task<BaseResult> DeleteUserDevice(long userId)
        {
            var result = await _applicationDbContext.UserDevices.Where(x => x.UserId == userId).ExecuteDeleteAsync();
            if (result > 0)
                return new BaseResult(new { }, true, Messages.Logout);
            return new BaseResult(new { }, false, Messages.Logout_Fail);
        }

        private async Task<BaseResult> UpdateUserDevice(UserDevice device)
        {
            var result = await _applicationDbContext.UserDevices.Where(x => x.UserId == device.UserId).ExecuteUpdateAsync(s =>
            s.SetProperty(x => x.DeviceInfo, device.DeviceInfo)
            .SetProperty(x => x.FirebaseToken, device.FirebaseToken)
            );
            if (result > 0)
                return new BaseResult(new { }, true, string.Empty);
            return new BaseResult(new { }, false, string.Empty);
        }
    }
}
