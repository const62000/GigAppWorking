using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Facilities;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;
using Mapster;
using GigApp.Application.Interfaces.Auth;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Facilities.Commands
{
    public class AddFacility
    {
        public class Command : ICommand
        {
            public string Name { get; set; } = string.Empty;
            public string Auth0Id { get; set; } = string.Empty;
            public AddressRequest Address { get; set; } = new AddressRequest(string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, string.Empty);

        }
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AddressRequest, Address>()
                .Map(des => des.City, src => src.City)
                .Map(des => des.Country, src => src.Country)
                .Map(des => des.Latitude, src => src.Latitude)
                .Map(des => des.Longitude, src => src.Longitude)
                .Map(des => des.StreetAddress1, src => src.StreetAddress1)
                .Map(des => des.StreetAddress2, src => src.StreetAddress2);

            config.NewConfig<Command, Client>()
                .Map(des => des.Name, src => src.Name);

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Address).NotNull();
                RuleFor(x => x.Address.Longitude).NotNull();
                RuleFor(x => x.Address.Latitude).NotNull();
                RuleFor(x => x.Address.StreetAddress1).NotEmpty();
                RuleFor(x => x.Address.City).NotEmpty();
                RuleFor(x => x.Address.Country).NotEmpty();
            }
        }
        internal sealed class Handler(IValidator<Command> _validator, IMapper _mapper, IFacilitiesRepository _facilitiesRepository, IAuthRepository _authRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var client = _mapper.Map<Client>(request);
                client.Addresses.Add(_mapper.Map<Address>(request.Address));
                var isAdmin = await _authRepository.UserHasRoleByAuth0Id($"auth0|{request.Auth0Id}", "Admin");
                var result = await _facilitiesRepository.AddFacility(client, isAdmin ? string.Empty : request.Auth0Id);
                return result;
            }
        }
    }
}
