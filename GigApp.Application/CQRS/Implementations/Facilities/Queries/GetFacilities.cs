using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Facilities;
using GigApp.Application.Interfaces.Vendors;
using GIgApp.Contracts.Responses;
using GigApp.Application.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Facilities.Queries
{
    public class GetFacilities
    {
        public class Query : IQuery
        {
            public string Auth0Id { get; set; } = string.Empty;
        }
        internal sealed class Handler(IFacilitiesRepository _facilitiesRepository, IAuthRepository _authRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var isAdmin = await _authRepository.UserHasRoleByAuth0Id($"auth0|{request.Auth0Id}", "Admin");
                var result = await _facilitiesRepository.GetFacilities(request.Auth0Id, isAdmin);
                return result;
            }
        }
    }
}
