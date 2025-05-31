using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Facilities.Queries.GetFacility;

namespace GigApp.Api.Features.Facilities
{
    public class GetFacility : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Facilities_EndPoint + "/{id}", async (long Id, ISender sender) =>
            {
                var result = await sender.Send(new Query { Id = Id });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
