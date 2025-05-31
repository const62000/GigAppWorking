

using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Vendors;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using GigApp.Application.Interfaces.Auth;
using Mapster;

namespace GigApp.Application.CQRS.Implementations.Vendors.Commands
{
    public class AddVendor
    {
        public class Command : ICommand
        {
            public string? ServicesOffered { get; set; }
            public string? Name { get; set; } = string.Empty;
            public string? Certifications { get; set; } = "Certified";
            public string Auth0Id { get; set; } = string.Empty;
            public string? Status { get; set; } = "Active";
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.ServicesOffered).NotEmpty();
                RuleFor(x => x.Certifications).NotEmpty();
                RuleFor(x => x.Auth0Id).NotEmpty();
                RuleFor(x => x.Status).NotEmpty();
            }
        }
        internal sealed class Handler(IVendorRepository _vendorRepository, IValidator<Command> _validator, IAuthRepository _authRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var vendor = request.Adapt<Vendor>();
                if (await _authRepository.UserHasRoleByAuth0Id($"auth0|{request.Auth0Id}", "Admin"))
                {
                    var result = await _vendorRepository.AddVendor(vendor, string.Empty);
                    return result;
                }
                var reult = await _vendorRepository.AddVendor(vendor, request.Auth0Id);
                if (reult.Status)
                {
                    var result = await _authRepository.AssignRole($"auth0|{request.Auth0Id}", "Vendor");
                    if (result.Status)
                        return reult;
                    else
                        return result;
                }
                return reult;
            }
        }
    }
}
