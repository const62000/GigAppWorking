using Carter;
using GigApp.Contracts.Requests.TimeSheet;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.TimeSheets.Commands.AddTimeSheetLocation;

namespace GigApp.Api.Features.TimeSheet;

public class AddTimeSheetLocation : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.TimeSheetLocation, async (TimeSheetLocationRequest request,ISender sender) =>
        {
            var command = request.Adapt<Command>();
            var result = await sender.Send(command);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization();
    }
}