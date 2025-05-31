using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Vendors;
using GIgApp.Contracts.Responses;
using GigApp.Application.Interfaces.Auth;
namespace GigApp.Application.CQRS.Implementations.Vendors.Queries;

public class GetVendor
{
    public class Query : IQuery
    {
        public int VendorId { get; set; }
        public string Auth0Id { get; set; } = string.Empty;
    }
    internal sealed class Handler(IVendorRepository _vendorRepository, IAuthRepository _authRepository) : IQueryHandler<Query>
    {
        public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var isAdmin = await _authRepository.UserHasRoleByAuth0Id($"auth0|{request.Auth0Id}", "Admin");
            var vendor = await _vendorRepository.GetVendorById(request.VendorId, request.Auth0Id, isAdmin);
            return vendor;
        }
    }
}