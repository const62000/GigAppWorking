using Carter;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Users.Queries.ChangeFreelancerStatus;

namespace GigApp.Api.Features.Users;

public class ChangeFreelancerStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(EndPoints.Freelancers + "/status/{userId}/{disabled}", async (long userId, bool disabled, ISender sender) =>
        {
            var result = await sender.Send(new Query { UserId = userId, Disabled = disabled });
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(result);
        }).RequireAuthorization("Admin");
    }
}

