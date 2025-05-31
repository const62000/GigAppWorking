using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIgApp.Contracts.Enums;
using GigApp.Application.CQRS.Implementations.Users.Queries;

namespace GigApp.Application.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<BaseResult> AddAuth0Id(long id, string auth0Id);
        Task<BaseResult> AddUser(User user, List<UserType> userTypes);
        Task<long> GetUserId(string auth0Id);
        Task<User> GetUserById(long id);
        Task<BaseResult> DeleteUser(long userId);
        Task<BaseResult> GetCurrentUser(string auth0Id);
        Task<BaseResult> UpdateUserProfile(string auth0Id, string picture);
        Task<BaseResult> UpdateUserStripeAccountId(long userId, string stripeAccountId);
    }
}
