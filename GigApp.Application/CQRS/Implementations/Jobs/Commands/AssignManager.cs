using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class AssignManager
    {
        public class Command : ICommand
        {
            public long JobId { get; set; }
            public string Auth0Id { get; set; } = string.Empty;
            public long UserId { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.JobId).GreaterThan(0).WithMessage("JobId must be a valid Job Id.");
                RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be a valid User Id.");
            }
        }
        public class Handler : ICommandHandler<Command>
        {
            private readonly IJobRepository _jobRepository;
            private readonly IValidator<Command> _validator;

            public Handler(IJobRepository jobRepository, IValidator<Command> validator)
            {
                _jobRepository = jobRepository;
                _validator = validator;
            }
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    // Validate the request
                    var validationResult = _validator.Validate(request);
                    if (!validationResult.IsValid)
                    {
                        return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                    }

                    var assignManagerRequest = new AssignManagerRequest(
                        request.JobId,
                        request.Auth0Id,
                        request.UserId
                    );

                    var result = await _jobRepository.AssignJobManagerAsync(assignManagerRequest);
                    if (!result.Status)
                    {
                        return new BaseResult(new {}, false, result.Message);
                    }

                    // Return successful result
                    return new BaseResult(result.Data, true, "Job manager assigned successfully");
                }
                catch (Exception ex)
                {
                    return new BaseResult(0, false, "An unexpected error occurred: " + ex.Message);
                }
            }
        }
    }
}
