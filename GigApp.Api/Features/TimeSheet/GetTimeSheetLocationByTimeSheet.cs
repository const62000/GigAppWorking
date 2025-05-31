using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.TimeSheets.Queries.GetTimeSheetLocationByTimeSheet;

namespace GigApp.Api.Features.TimeSheet;

public class GetTimeSheetLocationByTimeSheet : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{EndPoints.TimeSheetLocation}/{{timeSheetId}}", async (long timeSheetId, ISender sender) =>
        {
            var query = new Query { TimeSheetId = timeSheetId };
            var result = await sender.Send(query);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization();
    }
}