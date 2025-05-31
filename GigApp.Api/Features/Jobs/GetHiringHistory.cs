using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Jobs.Queries.GetHiringHistory;

namespace GigApp.Api.Features.Jobs
{
    public class GetHiringHistory : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Hiring_Job + "/{id}", async (long Id,ISender sender) =>
            {
                var result = await sender.Send(new Query { JobId = Id });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
