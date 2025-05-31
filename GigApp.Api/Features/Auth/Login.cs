using Carter;
using FluentValidation;
using GIgApp.Contracts.Requests.Login;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Auth.Commands.Login;

namespace GigApp.Api.Features.Auth
{
    public class Login : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Login, async (LoginRequest request, ISender send) =>
            {
                var query = request.Adapt<Query>();
                var result = await send.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });

            // app.MapGet("/api/hello", () =>
            // {
            //     var currentTime = DateTime.UtcNow;
            //     var message = $"Hello from your awesome API! Current time is: {currentTime:yyyy-MM-ddTHH:mm:ssZ}";
            //     return Results.Ok(new { Message = message });
            // });
        }
    }
}
