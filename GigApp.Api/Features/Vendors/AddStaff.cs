using System.Security.Claims;
using Mapster;
using MediatR;
using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using static GigApp.Application.CQRS.Implementations.Vendors.Commands.AddStaff;
using GIgApp.Contracts.Requests.Vendor;

namespace GigApp.Api.Features.Vendors;

public class AddStaff : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.Staff_EndPoint, async (StaffRequest request, HttpContext ctx, ISender sender) =>
        {
            var userClaims = ctx.User;
            var command = request.Adapt<Command>();
            command.Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
            var result = await sender.Send(command);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization();
    }
}