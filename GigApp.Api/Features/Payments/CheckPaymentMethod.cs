using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Payments.Queries.CheckPaymentMethod;
using Carter;
using GIgApp.Contracts.Shared;
using GIgApp.Contracts.Responses;
using MediatR;

namespace GigApp.Api.Features.Payments;

public class CheckPaymentMethod : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(EndPoints.CheckPaymentMethod, async (HttpContext ctx, ISender sender) =>
        {
            var userClaims = ctx.User;
            var query = new Query();
            query.Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
            var result = await sender.Send(query);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}