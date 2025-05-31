using Carter;
using GIgApp.Contracts.Requests.Vendor;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Vendors.Queries.DeleteVendors;

namespace GigApp.Api.Features.Vendors
{
    public class DeleteVendors : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Vendor_EndPoint + "/remove", async (DeleteVendorsRequest request,ISender sender) =>
            {
                var query = new Query { vendorIds = request.VendorIds};
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
