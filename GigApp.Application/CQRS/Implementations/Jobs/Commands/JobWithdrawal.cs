using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class JobWithdrawal
    {
        public class Command : ICommand
        {
            public string Auth0Id { get; set; } = string.Empty;
            public long JobApplicationId { get; set; }
            public string WithdrawalReason { get; set; } = string.Empty;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.WithdrawalReason).NotEmpty().WithMessage("Withdrawal reason is required."); 
            }
        }
        public class Handler : ICommandHandler<Command>
        {
            private readonly IJobApplicationRepository _jobApplicationRepository;
            private readonly IValidator<Command> _validator;
            public Handler(IJobApplicationRepository jobApplicationRepository, IValidator<Command> validator)
            {
                _jobApplicationRepository = jobApplicationRepository;
                _validator = validator;
            }
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var validationResult = _validator.Validate(request);
                    if (!validationResult.IsValid)
                    {
                        return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                    }

                    var withdrawalRequest = new JobWithdrawalRequest
                    (
                        request.JobApplicationId,
                        request.WithdrawalReason
                    );
                    var result = await _jobApplicationRepository.WithdrawalJobAsync(withdrawalRequest);
                    if (!result.Status)
                    {
                        return new BaseResult(new { }, false, result.Message);  // Return the failure message from the repository
                    }
                    return new BaseResult(null, true, "Job withdrawal updated successfully!");
                }
                catch (Exception ex)
                {
                    return new BaseResult(new { }, false, $"An error occurred: {ex.Message}");
                }
            } 
        }
    }
}
