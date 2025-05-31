using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Payments;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Payments.Commands
{
    public class RaiseDispute
    {
        public class Command : ICommand
        {
            public string Auth0Id { get; set; } = string.Empty;
            public string Type { get; set; } = null!;

            public string Reason { get; set; } = null!;

            public string Description { get; set; } = null!;

            public string? Attachment { get; set; }
            public int? HiredJobId { get; set; }

            public long? TimeSheetId { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Type).NotEmpty();
                RuleFor(x => x.Auth0Id).NotEmpty();
                RuleFor(x => x.Reason).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
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
                var dispute = request.Adapt<Dispute>();
                var result = await _disputeRepository.RaiseDispute(dispute, request.Auth0Id);
                return result;
            }
        }

    }
}
