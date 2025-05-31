using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Addresses.Queries.GetCountries;

namespace GigApp.Api.Features.Addresses
{
    public class GetCountries : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Counties, async (ISender sender) =>
            {
                var result = await sender.Send(new Query { });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });
        }
    }
}
