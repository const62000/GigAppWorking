using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Globalization;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Vendors.Queries.GetVendor;

namespace GigApp.Api.Features.Vendors;

public class GetVendor : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(EndPoints.Vendor_EndPoint + "/{vendorId}", async (int vendorId, HttpContext ctx, ISender sender) =>
        {
            var userClaims = ctx.User;
            var query = new Query
            {
                VendorId = vendorId,
                Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty
            };
            var result = await sender.Send(query);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}