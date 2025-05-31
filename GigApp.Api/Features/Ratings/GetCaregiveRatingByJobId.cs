using Carter;
using GIgApp.Contracts.Shared;
using MediatR;
using GIgApp.Contracts.Responses;
using static GigApp.Application.CQRS.Implementations.Ratings.Queries.GetCaregiveRatingByJobId;

namespace GigApp.Api.Features.Ratings;

public class GetCaregiveRatingByJobId : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{EndPoints.CaregiverRatingByJobId}", async (long caregiverId, long jobId, ISender sender) =>
        {
            var result = await sender.Send(new Query { CaregiverId = caregiverId, JobId = jobId });
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}