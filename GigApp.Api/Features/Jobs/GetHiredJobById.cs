using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Jobs.Queries.GetHiredJobById;

namespace GigApp.Api.Features.Jobs
{
    public class GetHiredJobById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Hiring + "/{id}", async (int Id,ISender sender) =>
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