using Carter;
using GIgApp.Contracts.Requests.BankAccount;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Users.Queries.DeleteUser;
namespace GigApp.Api.Features.Users;

public class DeleteUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(EndPoints.Users + "/{userId}", async (long userId, ISender sender) =>
        {
            var result = await sender.Send(new Query { Id = userId });
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization("Admin");
    }
}