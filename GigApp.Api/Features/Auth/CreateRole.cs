using GIgApp.Contracts.Requests.Users;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using Carter;
using MapsterMapper;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Auth.Commands.SignUp;

namespace GigApp.Api.Features.Auth;

public class CreateRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.Role, async (RolesRequest request, ISender send) =>
        {
            var command = request.Adapt<Command>();
            var result = await send.Send(command);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}