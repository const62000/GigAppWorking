using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Payments.Queries.DeletePaymentMethod;

namespace GigApp.Api.Features.Payments
{
    public class DeletePaymentMethod : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(EndPoints.PaymentMethod+"/{id}", async (int id,HttpContext ctx, ISender sender) =>
            {
                var userClaims = ctx.User;
                var query = new Query { PaymentMethodId = id,Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
