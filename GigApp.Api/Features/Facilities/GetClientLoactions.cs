using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Facilities.Queries.GetClientLocations;

namespace GigApp.Api.Features.Facilities
{
    public class GetClientLocations : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Facilities_EndPoint + "/{clientId}/locations", async (long clientId, ISender sender) =>
            {
                var result = await sender.Send(new Query { Id = clientId });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
