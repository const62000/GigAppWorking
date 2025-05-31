using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using GigApp.Contracts.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.UpdateJob;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class UpdateJob
    {
        public class Command : ICommand
        {
            public long JobId { get; set; }
            public string Title { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public string? Requirements { get; set; }

            public DateOnly Date { get; set; }

            public TimeOnly Time { get; set; }

            public decimal Rate { get; set; }
            public JobType JobType { get; set; }
            public int HoursPerWeek { get; set; }

            public string Status { get; set; } = string.Empty;
            public DateTime StartedDate { get; set; }
            public DateTime EndedDate { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.JobId).GreaterThan(0).WithMessage("JobId must be a valid Job Id.");
                RuleFor(x => x.Title).NotEmpty().WithMessage("Title must not be an empty.");
                RuleFor(x => x.StartedDate).NotNull().WithMessage("Started date is required");
                RuleFor(x => x.EndedDate).NotNull().WithMessage("Ended date is required");
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

                    // Map Command to UpdateJobRequest
                    var updateJobRequest = new UpdateJobRequest(
                        request.JobId,
                        request.Title,
                        request.Description,
                        request.Requirements,
                        request.Date,
                        request.Time,
                        request.Rate,
                        request.Status,
                        request.JobType,
                        request.HoursPerWeek,
                        request.StartedDate,
                        request.EndedDate
                    );

                    // Update the job
                    var updateJobResult = await _jobRepository.UpdateJobAsync(updateJobRequest);
                    if (!updateJobResult.Status)
                    {
                        return new BaseResult(new {}, false, updateJobResult.Message);
                    }

                    // Return successful result
                    return new BaseResult(updateJobResult.Data, true, "Job data updated successfully!");
                }
                catch (Exception ex)
                {
                    // Handle any other exceptions
                    return new BaseResult(0, false, "An unexpected error occurred: " + ex.Message);
                }
            }
        }
    }
}


