using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.ViewJobApplication;

namespace GigApp.Api.Features.Jobs
{
    public class ViewJobApplication : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.View_Job_Application, async (long id, ISender sender) =>
            {
                var result = await sender.Send(new Command { ApplicationId = id });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
