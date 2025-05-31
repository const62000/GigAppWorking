using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Payments;
using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Payments.Commands
{
    public class ProcessDispute
    {
        public class Command : ICommand
        {
            public string Auth0Id { get; set; } = string.Empty;
            public long DisputeId { get; set; }
            public string ActionMessage { get; set; } = null!;

            public string ActionStatus { get; set; } = null!;
            public string? Attachment { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Auth0Id).NotEmpty();
                RuleFor(x => x.DisputeId).NotEmpty();
            }
        }
        internal class Handler(IDisputeRepository _disputeRepository, IValidator<Command> _validator) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var disputeAction = request.Adapt<DisputeAction>();
                var result = await _disputeRepository.ProcessDispute(disputeAction, request.Auth0Id);
                return result;
            }
        }
    }
}
