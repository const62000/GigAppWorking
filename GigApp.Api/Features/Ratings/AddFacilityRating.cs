using Carter;
using GIgApp.Contracts.Requests.Ratings;
using GIgApp.Contracts.Responses;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Ratings.Commands.CreateFacilityRating;
using GIgApp.Contracts.Shared;
namespace GigApp.Api.Features.Ratings;

public class AddFacilityRating : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.FacilityRating, async (CreateFacilityRatingRequest request, HttpContext ctx, ISender sender) =>
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