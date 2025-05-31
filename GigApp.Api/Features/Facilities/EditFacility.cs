using Carter;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Facilities.Commands.EditFacility;


namespace GigApp.Api.Features.Facilities
{
    public class EditFacility : ICarterModule
    {
        public void Register(TypeAdapterConfig config)

        {
            config.NewConfig<ClientRequest, Command>()
                .Map(des => des.Name, src => src.Name)
                .Map(des => des.Address, src => src.Address);
        }
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut(EndPoints.Facilities_EndPoint + "/{id}", async (long id, ClientRequest request, ISender sender, HttpContext ctx, IMapper _mapper) =>
            {
                var userClaims = ctx.User;
                var command = _mapper.Map<Command>(request);

                command.Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "") ?? string.Empty;
                command.Id = id;
                var result = await sender.Send(command);
                if (result.Status)
                    return Results.Ok(result);

                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            }).RequireAuthorization();
        }
    }
}
