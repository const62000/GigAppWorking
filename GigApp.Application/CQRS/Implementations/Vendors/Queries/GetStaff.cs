using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Vendors;
using GIgApp.Contracts.Responses;
using GigApp.Application.Interfaces.Auth;

namespace GigApp.Application.CQRS.Implementations.Vendors.Queries;

public class GetStaff
{
    public class Query : IQuery
    {
        public string Auth0Id { get; set; }
    }

    public class Handler(IVendorRepository _vendorRepository, IAuthRepository _authRepository) : IQueryHandler<Query>
    {
        public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var isAdmin = await _authRepository.UserHasRoleByAuth0Id($"auth0|{request.Auth0Id}", "Admin");
            var result = await _vendorRepository.GetStaff(request.Auth0Id, isAdmin);
            return result;
        }
    }
}