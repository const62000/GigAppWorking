using Carter;
using GIgApp.Contracts.Requests.Users;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Users.Commands.UpdateUserProfile;

namespace GigApp.Api.Features.Users
{
    public class UpdateUserProfile : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Update_User_Profile, async (UpdateProfileRequest request,HttpContext ctx ,ISender sender) =>
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
}
