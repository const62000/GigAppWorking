using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Auth
{
    public interface IAuthRepository
    {
        Task<BaseResult> Login(string username, string password);
        Task<BaseResult> SignUp(string email, string password);
        Task<BaseResult> RegisterFreelancer(string email, string password);
        Task<BaseResult> VendorLogin(string email, string password);
        Task<BaseResult> CreateRole(string name, string description);
        Task<BaseResult> AssignRole(string auth0UserId, string roleName);
        Task<BaseResult> DeleteUser(string auth0UserId);
        Task<bool> UserHasRoleByAuth0Id(string auth0UserId, string roleName);
        Task<bool> UserHasRoleByEmail(string email, string roleName);
    }
}

