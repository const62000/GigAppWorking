using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Users
{
    public interface IUserDeviceRepository
    {
        Task<BaseResult> AddUserDevice(UserDevice device, string email);
        Task<BaseResult> DeleteUserDevice(long userId);
    }
}
