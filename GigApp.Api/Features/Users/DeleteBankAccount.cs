using Carter;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Users.Queries.DeleteBankAccount;

namespace GigApp.Api.Features.Users
{
    public class DeleteBankAccount : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(EndPoints.BackAccount + "/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new Query { Id = id });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
