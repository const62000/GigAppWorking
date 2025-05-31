using Carter;
using GIgApp.Contracts.Requests.Vendor;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Vendors.Commands.AssignVendorManager;

namespace GigApp.Api.Features.Vendors;

public class AssignVendorManager : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.VendorManager_EndPoint, async (AssignVendorManagerRequest request, HttpContext ctx, ISender sender) =>
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