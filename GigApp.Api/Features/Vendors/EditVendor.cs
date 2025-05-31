using Carter;
using GIgApp.Contracts.Requests.Vendor;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Vendors.Commands.EditVendor;

namespace GigApp.Api.Features.Vendors;

public class EditVendor : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(EndPoints.Vendor_EndPoint + "/{vendorId}", async (VendorRequest request, int vendorId, HttpContext ctx, ISender sender) =>
        {
            var command = request.Adapt<Command>();
            command.VendorId = vendorId;
            var result = await sender.Send(command);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}