using Carter;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Requests.Vendor;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Facilities.Commands.AssignJobManager;

namespace GigApp.Api.Features.Facilities;

public class AssignJobManager : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.Facilities_EndPoint + "/{clientId}/job-managers", async (AssignJobManagerRequest request, HttpContext ctx, ISender sender) =>
        {
            var command = request.Adapt<Command>();
            var result = await sender.Send(command);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}