using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Facilities;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;
using Mapster;
using GigApp.Application.Interfaces.Auth;
using MapsterMapper;

namespace GigApp.Application.CQRS.Implementations.Facilities.Commands
{
    public class AddClientLoaction
    {
        public class Command : ICommand
        {
            public string LocationName { get; set; } = string.Empty;
            public string Auth0Id { get; set; } = string.Empty;
            public AddressRequest Address { get; set; } = new AddressRequest(string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, string.Empty);
            public long ClientId { get; set; }

        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.LocationName).NotEmpty().WithMessage("Location Name is required");
                RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client Id is required");
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
                var clientLocation = _mapper.Map<ClientLocation>(request);
                clientLocation.Address = _mapper.Map<Address>(request.Address);
                var isAdmin = await _authRepository.UserHasRoleByAuth0Id($"auth0|{request.Auth0Id}", "Admin");
                var result = await _facilitiesRepository.AddClientLocation(clientLocation, isAdmin ? string.Empty : request.Auth0Id);
                return result;
            }
        }
    }
}
