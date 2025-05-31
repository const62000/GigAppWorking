using Carter;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Requests.Vendor;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Jobs.Queries.CompleteJob;

namespace GigApp.Api.Features.Jobs;

public class CompleteJob : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(EndPoints.CompleteJob, async (long jobId, ISender sender) =>
        {
            var query = new Query { JobId = jobId };
            var result = await sender.Send(query);
            if(result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));  
        }).RequireAuthorization();
    }
}
