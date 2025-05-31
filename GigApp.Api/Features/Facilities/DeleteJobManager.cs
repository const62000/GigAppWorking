using Carter;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Facilities.Queries.DeleteJobManager;

namespace GigApp.Api.Features.Facilities;

public class DeleteJobManager : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(EndPoints.Facilities_EndPoint + "/{clientId}/job-managers/{userId}", async (long clientId, long userId, HttpContext ctx, ISender sender) =>
        {
            var result = await sender.Send(new Query { ClientId = clientId, UserId = userId });
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}