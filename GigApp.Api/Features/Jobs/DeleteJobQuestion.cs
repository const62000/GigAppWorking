using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Jobs.Queries.DeleteJobQuestions;

namespace GigApp.Api.Features.Jobs
{
    public class DeleteJobQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(EndPoints.Delete_Job_Question, async (long id, ISender sender) =>
            {
                var query = new Query { Ids = new List<long> { id } };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
