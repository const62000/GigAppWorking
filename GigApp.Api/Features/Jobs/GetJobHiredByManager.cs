using Carter;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Jobs.Queries.GetHiredJobByHiredManager;

namespace GigApp.Api.Features.Jobs
{
    public class GetJobHiredByManager : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Hiring_Manager , async (HttpContext ctx,ISender sender) =>
            {
                var userClaims = ctx.User;
                string auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
                var result = await sender.Send(new Query { Auth0Id = auth0Id });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}