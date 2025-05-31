using Carter;
using GIgApp.Contracts.Requests.TimeSheet;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.TimeSheets.Commands.ClockOut;

namespace GigApp.Api.Features.TimeSheet
{
    public class ClockOut : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.ClockOut, async (ClockInOutRequest request, HttpContext ctx, ISender sender) =>
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
