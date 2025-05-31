using Carter;
using GIgApp.Contracts.Requests.Ratings;
using GIgApp.Contracts.Responses;
using Mapster;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Ratings.Commands.CreateCaregiverRating;

namespace GigApp.Api.Features.Ratings;

public class AddCaregiverRating : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.CaregiverRating, async (CaregiverRatingRequest request, HttpContext ctx, ISender sender) =>
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