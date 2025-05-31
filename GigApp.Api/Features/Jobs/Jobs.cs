using Azure.Core;
using Carter;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.CreateJob;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.JobDetails;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.DeleteJob;
using GigApp.Application.CQRS.Implementations.Jobs.Queries;
using System.Security.Claims;

namespace GigApp.Api.Features.Jobs
{
    public class Jobs : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.CreateJob, async ( CreateJobRequest request, HttpContext ctx, ISender send) =>
            {
                var userClaims = ctx.User;
                var command = request.Adapt<Command>();
                command.Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
                var result = await send.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();

            app.MapGet(EndPoints.JobDetails, async (long jobId, ISender send) =>
            {
                var query = new JobDetails.Query { JobId = jobId };
                var result = await send.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });

            app.MapPost(EndPoints.JobList, async (JobSearchRequest request, ISender send) =>
            {
                var command = request.Adapt<ListJobs.Command>();
                var result = await send.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });

            app.MapPut(EndPoints.UpdateJob, async (UpdateJobRequest request, ISender send) =>
            {
                var command = request.Adapt<UpdateJob.Command>();
                var result = await send.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });

            //Delete API
            app.MapDelete(EndPoints.DeleteJob, async (long jobId, ISender send) =>
            {
                var query = new DeleteJob.Query { JobId = jobId };
                var result = await send.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });

            //Job By Status
            app.MapGet(EndPoints.JobsByStatus, async(string status, ISender sender) => 
            {
                var query = new JobsByStatus.Query {  Status = status };
                var result = await sender.Send(query);
                return result;
            });

            //Delete Jobs API
            app.MapPost(EndPoints.DeleteJobs, async (DeleteJobsRequest request, ISender sender) =>
            {
                var query = new DeleteJobs.Query { Ids = request.Ids };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));

            });
        }
    }
}
