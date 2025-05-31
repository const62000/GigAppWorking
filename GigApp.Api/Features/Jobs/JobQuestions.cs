using Carter;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.CQRS.Implementations.Jobs.Queries;
using GIgApp.Contracts.Requests.JobQuestion;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;

namespace GigApp.Api.Features.Jobs
{
    public class JobQuestions : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.CreateJobQuestion, async (CreateJobQuestionRequest request, ISender send) =>
            {
                var command = request.Adapt<CreateJobQuetion.Command>();
                var result = await send.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });

            app.MapGet(EndPoints.JobQuestionByJobId, async (long jobId, ISender sender) => 
            {
                var query = new JobQuestionsById.Query { JobId = jobId };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });

            app.MapGet(EndPoints.JobQuestionnaireAnswersByJobApplicationId, async (int jobApplicationId, ISender sender) =>
            {
                var query = new JobQuestionnaireAnswersByJobApplicationId.Query {  JobApplicationId = jobApplicationId };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });


        }
    }
}
 