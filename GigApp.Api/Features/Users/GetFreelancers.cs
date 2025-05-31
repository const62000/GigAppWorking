
using Carter;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Users.Queries.GetFreelancers;
using GIgApp.Contracts.Responses;

namespace GigApp.Api.Features.Users
{
    public class GetFreelancers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.Freelancers, async (ISender send) =>
            {
                return await send.Send(new Query());
            });
        }
    }
}
