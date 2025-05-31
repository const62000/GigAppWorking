using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Notifications;
using GIgApp.Contracts.Requests.JobQuestion;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class CreateJobQuetion
    {
        public class Command : ICommand
        {
            public long JobId { get; set; }
            public string Title { get; set; } = string.Empty;
            public DateTime CurrentTimeStamp { get; set; } = DateTime.Now;
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required."); ;
            }
        }
        public class Handler : ICommandHandler<Command>
        {
            private readonly IJobQuestionRepository _jobQuestionRepository;
            private readonly IValidator<Command> _validator;
             
            public Handler(IJobQuestionRepository jobQuestionRepository, IValidator<Command> validator, INotificationRepository notificationRepository)
            {
                _jobQuestionRepository = jobQuestionRepository;
                _validator = validator;
            }
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                // Map Command to CreateJobRequest
                var createJobQuestionRequest = new CreateJobQuestionRequest(
                    request.JobId,
                    request.Title,
                    request.CurrentTimeStamp                  
                );

                try
                {
                    // Attempt to create the job question
                    var createJobQuestionResult = await _jobQuestionRepository.CreateJobQuestionAsync(createJobQuestionRequest);

                    // Check the result and return accordingly
                    if (!createJobQuestionResult.success)
                    {
                        return new BaseResult(new { }, false, "Failed to create job question.");
                    }

                    return new BaseResult(new { createJobQuestionResult }, true, "Job Question created successfully!");
                }
                catch (Exception ex)
                {
                    return new BaseResult(new {ex.Message}, false, "An error occurred while processing your request.");
                }
            }
        }

    }
}
