using Azure.Core;
using Carter;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.CreateJobApplication;
using static GigApp.Application.CQRS.Implementations.Jobs.Queries.GetJobApplication;

namespace GigApp.Api.Features.Jobs
{
    public class JobApplications : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //Create Job Application API
            app.MapPost(EndPoints.CreateJobApplication, async (long jobId, CreateJobApplicationRequest request,HttpContext ctx, ISender send) =>
            {
                var userClaims = ctx.User;
                var command = request.Adapt<Command>();
                command.Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
                command.JobId = jobId;  
                var result = await send.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();

            //Create Get by Id API
            app.MapGet(EndPoints.GetJobApplicationById, async(long jobId, ISender send) =>
            {
                var query = new Query { JobId = jobId };
                var result = await send.Send(query);
                return result;

            });
        }

        


    }
}
