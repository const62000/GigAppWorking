using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Users.Queries.GetBankAccount;

namespace GigApp.Api.Features.Users
{
    public class GetBankAccount : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.BackAccount, async (HttpContext ctx, ISender sender) =>
            {
                var userClaims = ctx.User;
                string auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "")??string.Empty;
                var result = await sender.Send(new Query { Auth0Id = auth0Id});
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
