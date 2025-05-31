using Carter;
using GIgApp.Contracts.Shared;
using System.Security.Claims;
using GigApp.Contracts.Enums;
using GIgApp.Contracts.Responses;
using MediatR;
using static GigApp.Application.CQRS.Implementations.TimeSheets.Commands.ChangeTimeSheetApprovalStatus;

namespace GigApp.Api.Features.TimeSheet;

public class ChangeTimeSheetApprovalStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{EndPoints.ChangeTimeSheetApprovalStatus}/{{timeSheetId}}/{{approvalStatus}}", async (long timeSheetId, TimeSheetApprovalStatus approvalStatus, HttpContext ctx, ISender sender) =>
        {
            var userClaims = ctx.User;
            var command = new Command { TimeSheetId = timeSheetId, Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty, ApprovalStatus = approvalStatus };
            var result = await sender.Send(command);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization();
    }
}