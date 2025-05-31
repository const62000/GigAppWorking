using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.TimeSheets.Queries.GetTimeSheetByHiredId;

namespace GigApp.Api.Features.TimeSheet
{
    public class GetTimeSheetByHiredId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.TimeSheet_Hired+"/{id}", async (int Id,HttpContext ctx, ISender sender) =>
            {
                var query = new Query {HiredId = Id };
                var result = await sender.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
