using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Interfaces.Users;
using Auth0.ManagementApi.Models;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Mapster;
using MapsterMapper;

namespace GigApp.Application.CQRS.Implementations.Auth.Commands;

public class AddRole
{
    public class Command : ICommand
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
    public class Handler : ICommandHandler<Command>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IValidator<Command> _validator;

        public Handler(IAuthRepository authRepository, IValidator<Command> validator)
        {
            _authRepository = authRepository;
            _validator = validator;
        }
        public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
            }
            await _authRepository.CreateRole(request.Name, request.Description);
            return new BaseResult(new { }, true, "Role created successfully");
        }
    }
}