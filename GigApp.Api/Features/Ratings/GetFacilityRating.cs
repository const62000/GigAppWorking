using Carter;
using GIgApp.Contracts.Shared;
using MediatR;
using GIgApp.Contracts.Responses;
using static GigApp.Application.CQRS.Implementations.Ratings.Queries.GetFacilityRatings;

namespace GigApp.Api.Features.Ratings;

public class GetFacilityRating : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{EndPoints.FacilityRating}/{{id}}", async (long id, ISender sender) =>
        {
            var result = await sender.Send(new Query { FacilityId = id });
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}