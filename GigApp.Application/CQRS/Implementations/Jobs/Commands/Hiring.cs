

using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using Mapster;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class Hiring
    {
        public class Command : ICommand
        {
            public long FreelancerId { get; set; }
            public long JobId { get; set; }
            public string Auth0Id {  get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty ;
            public string? Note { get; set; }

            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }
        }
        public class Validator : AbstractValidator<Command> 
        {
            public Validator() 
            {
                RuleFor(x => x.JobId).NotEqual(0);
                RuleFor(x => x.Auth0Id).NotEmpty();
                RuleFor(x=>x.FreelancerId).NotEqual(0);
                RuleFor(x=>x.Status).NotEmpty();
            }
        }
        internal sealed class Handler(IHiringRepository _hiringRepository,IValidator<Command> _validator) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var result = await _hiringRepository.HiringByJob(request.Adapt<GigApp.Domain.Entities.JobHire>(),request.Auth0Id);
                return result;
            }
        }
    }
}
