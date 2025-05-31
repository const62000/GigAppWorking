using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.TimeSheets.Queries.GetTimeSheetById;

namespace GigApp.Api.Features.TimeSheet
{
    public class GetTimeSheetById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.TimeSheet + "/{id}", async (long Id,  ISender sender) =>
            {
                var query = new Query { Id = Id };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
