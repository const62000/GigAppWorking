using Carter;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Facilities.Queries.DeleteFacilities;

namespace GigApp.Api.Features.Facilities
{
    public class DeleteFacilities : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Facilities_EndPoint + "/remove-multiple", async (DeleteClientRequest request, ISender sender) =>
            {
                var query = request.Adapt<Query>();
                var result = await sender.Send(query);
                if (result.Status)

                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
