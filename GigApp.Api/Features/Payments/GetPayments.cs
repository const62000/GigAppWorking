using Carter;
using System.Security.Claims;
using GigApp.Contracts.Requests.Payments;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using Mapster;

using static GigApp.Application.CQRS.Implementations.Payments.Queries.GetPayments;

namespace GigApp.Api.Features.Payments;

public class GetPayments : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.Payment, async (PaymentFilterRequest request,HttpContext ctx, ISender sender) =>
        {
            var userClaims = ctx.User;
            var query = request.Adapt<Query>();
            query.Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
            var result = await sender.Send(query);
            if(result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}