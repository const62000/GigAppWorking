using System.Security.Claims;
using GIgApp.Contracts.Shared;
using GigApp.Contracts.Requests.BankAccount;
using GIgApp.Contracts.Responses;
using Carter;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Users.Queries.VerifyBankAccount;

namespace GigApp.Api.Features.Users;

public class VerifyBankAccount : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.VerifyBankAccount, async (VerifyBankAccountRequest request,ISender sender) =>
        {   
            var result = await sender.Send(new Query {BankAccountId = request.Id,Amounts = request.Amounts});
            if (result.Status)  
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        }).RequireAuthorization();
    }
}
