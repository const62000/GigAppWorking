using Carter;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Requests.Vendor;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Jobs.Queries.DeleteJobQuestions;

namespace GigApp.Api.Features.Jobs
{
    public class DeleteJobQuestions : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Delete_Job_Questions, async (DeleteJobQuestionsRequest request, ISender sender) =>
            {
                var query = new Query { Ids = request.Ids };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
