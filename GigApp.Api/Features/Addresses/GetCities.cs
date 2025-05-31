using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Addresses.Queries.GetCities;

namespace GigApp.Api.Features.Addresses
{
    public class GetCities : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Cities, async (string country,ISender sender) =>
            {
                var result = await sender.Send(new Query {Country = country });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });
        }
    }
}
