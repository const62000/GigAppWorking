using Carter;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Auth.Commands.RegisterAdmin;

namespace GigApp.Api.Features.Auth;

public class RegisterAdmin : ICarterModule
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SignupRequest, Command>()
            .Map(des => des.FirstName, src => src.FirstName)
            .Map(des => des.LastName, src => src.LastName)
            .Map(des => des.Email, src => src.Email);
    }
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.RegisterAdmin, async (SignupRequest request, ISender send, IMapper _mapper) =>
        {
            var command = _mapper.Map<Command>(request);
            var result = await send.Send(command);
            if (result.Status)
                return Results.Ok(result);
            else
                return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
        });
    }
}