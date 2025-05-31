using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using GigApp.Application.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Users.Queries
{
    public class DeleteUser
    {
        public class Query : IQuery
        {
            public long Id { get; set; }
        }
        internal sealed class Handler(IUserRepository _userRepository, IAuthRepository _authRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserById(request.Id);
                var result = await _userRepository.DeleteUser(request.Id);
                if (result.Status)
                {
                    return await _authRepository.DeleteUser($"auth0|{user.Auth0UserId}");
                }
                return result;
            }
        }
    }
}