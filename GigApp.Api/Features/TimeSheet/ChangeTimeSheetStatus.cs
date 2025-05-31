using Carter;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Security.Claims;
using GIgApp.Contracts.Responses;
using GigApp.Contracts.Enums;
using static GigApp.Application.CQRS.Implementations.TimeSheets.Commands.ChangeTimeSheetStatus;

namespace GigApp.Api.Features.TimeSheet;

public class ChangeTimeSheetStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{EndPoints.ChangeTimeSheetStatus}/{{timeSheetId}}/{{status}}", async (long timeSheetId, TimeSheetStatus status, HttpContext ctx, ISender sender) =>
        {
            var userClaims = ctx.User;
            var command = new Command { TimeSheetId = timeSheetId, Status = status, Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty };
            var result = await sender.Send(command);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization();
    }
}