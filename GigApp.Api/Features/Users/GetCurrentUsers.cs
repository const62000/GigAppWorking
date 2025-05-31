using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Users.Queries.CurrentUser;

namespace GigApp.Api.Features.Users
{
    public class GetCurrentUsers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.CurrentUser, async (HttpContext ctx, ISender sender) =>
            {
                if (ctx.User == null)
                {
                    Results.BadRequest(new BaseFailResult(new { }, false, Messages.Logout_Fail));
                }
                string auth0Id = ctx.User?.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
                var query = new Query { Auth0Id = auth0Id };
                var result = await sender.Send(query);
                return result;
            }).RequireAuthorization();
        }
    }
}
