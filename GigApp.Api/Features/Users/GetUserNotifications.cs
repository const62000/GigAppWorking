using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Users.Queries.GetNotification;

namespace GigApp.Api.Features.Users
{
    public class GetUserNotifications : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Get_USer_Notifications, async (int page, int pageSize,HttpContext ctx, ISender sender) =>
            {
                var userClaims = ctx.User;
                var query = new Query {Page = page,PageSize = pageSize, Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
