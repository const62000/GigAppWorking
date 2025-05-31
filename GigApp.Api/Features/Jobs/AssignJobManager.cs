using Carter;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.AssignManager;

namespace GigApp.Api.Features.Jobs
{
    public class AssignJobManager : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.AssignJobManager, async(AssignManagerRequest request, HttpContext ctx, ISender send) => 
            {
                var command = request.Adapt<Command>();
                //command.Auth0Id = ctx.User?.FindFirstValue(ClaimTypes.NameIdentifier).Replace("auth0|", "");
                var result = await send.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });
        }
    }
}
