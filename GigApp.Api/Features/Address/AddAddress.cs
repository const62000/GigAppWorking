using Carter;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.AddressHandler.Commands.AddAddress;

namespace GigApp.Api.Features.Address
{
    public class AddAddress : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.GetAddress, async (AddressRequest request,HttpContext ctx, ISender sender) =>
            {
                if (ctx.User == null)
                {
                    Results.BadRequest(new BaseFailResult(new { }, false, Messages.Logout_Fail));
                }
                string Auth0Id = ctx.User?.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
                var command = request.Adapt<Command>();
                command.Auth0Id= Auth0Id;
                var result = await sender.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();

        }
    }
}
