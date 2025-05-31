using Carter;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.AddressHandler.Queries.GetAddress;

namespace GigApp.Api.Features.Address
{
    public class Address : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        { 
            app.MapGet(EndPoints.GetAddress, async (HttpContext ctx, ISender sender) =>
            {
                if (ctx.User == null)
                {
                    Results.BadRequest(new BaseFailResult(new { }, false, Messages.Logout_Fail));
                }
                string Auth0Id = ctx.User?.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
                var query = new Query { Id = Auth0Id };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();

        }
    }
}
