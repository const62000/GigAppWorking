using Carter;
using GIgApp.Contracts.Requests.BankAccount;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Users.Commands.UpdateBankAccount;

namespace GigApp.Api.Features.Users
{
    public class UpdateBankAccount : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut(EndPoints.BackAccount, async (BankAccountRequet request, ISender sender) =>
            {
                var command = request.Adapt<Command>();
                var result = await sender.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
