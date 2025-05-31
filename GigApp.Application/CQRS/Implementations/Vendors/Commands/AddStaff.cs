using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Vendors;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Mapster;

namespace GigApp.Application.CQRS.Implementations.Vendors.Commands;

public class AddStaff
{
    public class Command : ICommand
    {
        public string Auth0Id { get; set; } = string.Empty;
        public string Email { get; set; }
        public int VendorId { get; set; }
    }
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Auth0Id).NotEmpty();
        }
    }
    internal class Handler(IVendorRepository _vendorRepository, IValidator<Command> _validator) : ICommandHandler<Command>
    {
        public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
            if (request.VendorId > 0)
                return await _vendorRepository.AddStaff(request.VendorId, request.Email);
            else
                return await _vendorRepository.AddStaff(request.Auth0Id, request.Email);
        }

    }
}