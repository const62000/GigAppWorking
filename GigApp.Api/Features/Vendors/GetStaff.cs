using System.Security.Claims;
using Mapster;
using MediatR;
using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using static GigApp.Application.CQRS.Implementations.Vendors.Queries.GetStaff;

namespace GigApp.Api.Features.Vendors;

public class GetStaff : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(EndPoints.Staff_EndPoint, async (HttpContext ctx, ISender sender) =>
        {
            var userClaims = ctx.User;
            var query = new Query
            {
                Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty
            };
            var result = await sender.Send(query);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization();
    }
}
