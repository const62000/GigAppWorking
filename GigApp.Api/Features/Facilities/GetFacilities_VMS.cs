using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Facilities.Queries.GetFacilities_VMS;

namespace GigApp.Api.Features.Facilities
{
    public class GetFacilities_VMS : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Facilities_VMS, async (HttpContext ctx,ISender sender) =>
            {
                var query = new Query();
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });
        }
    }
}