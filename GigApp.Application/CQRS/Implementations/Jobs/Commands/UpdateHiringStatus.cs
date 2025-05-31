using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class UpdateHiringStatus
    {
        public class Command : ICommand
        {
            public long Id { get; set; }
            public string Status {  get; set; } = string.Empty;
            public string Note { get; set; } = string.Empty;
        } 
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEqual(0);
                RuleFor(x => x.Status).NotEmpty();
            }
        }
        internal sealed class Handler(IHiringRepository _hiringRepository, IValidator<Command> _validator) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var result = await _hiringRepository.UpdateHiredJobStatus(request.Adapt<Domain.Entities.JobHire>());
                return result;
            }
        }
    }
}
