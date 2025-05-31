using static GigApp.Application.CQRS.Implementations.Payments.Commands.AddStripeConnect;
using Carter;
using GIgApp.Contracts.Requests.Payments;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;

namespace GigApp.Api.Features.Payments;

public class AddStripeConnect : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.AddStripeConnect, async (HttpContext ctx, ISender sender) =>
        {
            var userClaims = ctx.User;
            var command = new Command();
            command.Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
            var result = await sender.Send(command);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization();
    }
}

