using Carter;
using GIgApp.Contracts.Shared;
using GIgApp.Contracts.Responses;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Ratings.Queries.GetCaregiverRatings;

namespace GigApp.Api.Features.Ratings;

public class GetCaregiverRating : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{EndPoints.CaregiverRating}/{{id}}", async (long id, ISender sender) =>
        {
            var result = await sender.Send(new Query { CaregiverId = id });
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}