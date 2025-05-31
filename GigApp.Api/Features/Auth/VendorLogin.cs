using Carter;
using FluentValidation;
using GIgApp.Contracts.Requests.Login;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Auth.Commands.VendorLogin;

namespace GigApp.Api.Features.Auth;

public class VendorLogin : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.VendorLogin, async (LoginRequest request, ISender send) =>
        {
            var query = request.Adapt<Query>();
            var result = await send.Send(query);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}