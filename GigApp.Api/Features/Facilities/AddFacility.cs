using Carter;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Facilities.Commands.AddFacility;

namespace GigApp.Api.Features.Facilities
{
    public class AddFacility : ICarterModule
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ClientRequest, Command>()
                .Map(des => des.Name, src => src.Name)
                .Map(des => des.Address, src => src.Address);
        }
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Facilities_EndPoint, async (ClientRequest request, ISender sender, HttpContext ctx, IMapper _mapper) =>
            {
                var userClaims = ctx.User;
                var command = _mapper.Map<Command>(request);
                command.Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
                var result = await sender.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
