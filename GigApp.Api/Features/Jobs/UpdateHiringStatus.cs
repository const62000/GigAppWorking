using Carter;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.UpdateHiringStatus;

namespace GigApp.Api.Features.Jobs
{
    public class UpdateHiringStatus : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Hiring_Status, async (HireRequest request, HttpContext ctx, ISender sender) =>
            {
                var command = request.Adapt<Command>();
                var result = await sender.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
