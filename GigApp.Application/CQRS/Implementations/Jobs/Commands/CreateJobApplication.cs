using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Notifications;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ICommand = GigApp.Application.CQRS.Abstractions.Command.ICommand;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class CreateJobApplication
    {
        public class Command : ICommand
        {
            public string Auth0Id { get; set; } = string.Empty;
            public long JobId { get; set; }
            public long FreelancerUserId { get; set; }
            public string Proposal { get; set; } = string.Empty;
            public decimal ProposedHourlyRate { get; set; }
            public List<AnswerInput> Answers { get; set; } = new List<AnswerInput>();
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Proposal).NotEmpty().WithMessage("Title is required.");
            }
        }
        public class Handler : ICommandHandler<Command>
        {
            private readonly INotificationRepository _notificationRepository;
            private readonly IJobApplicationRepository _jobApplicationRepository;
            private readonly IValidator<Command> _validator;

            public Handler(IJobApplicationRepository jobApplicationRepository, INotificationRepository notificationRepository, IValidator<Command> validator)
            {
                _jobApplicationRepository = jobApplicationRepository;
                _validator = validator;
                _notificationRepository = notificationRepository;
            }
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                // Map Command to CreateJobRequest
                var createJobApplicationRequest = new CreateJobApplicationRequest(
                    request.JobId,
                    request.FreelancerUserId,
                    request.Proposal,
                    request.ProposedHourlyRate,
                    request.Answers
                );

                var createJobApplicationResult = await _jobApplicationRepository.CreateJobApplicationAsync(createJobApplicationRequest, request.Auth0Id);
                if (createJobApplicationResult.ApplicationId != 0)
                    await _notificationRepository.JobAppliedNotification(createJobApplicationRequest.JobId, 1, request.Auth0Id);
                else
                {
                    return new BaseResult(new { }, false, createJobApplicationResult.Message);
                }
                return new BaseResult(new { ApplicationId = createJobApplicationResult.ApplicationId }, true, "Application submitted successfully"); // Make sure to return the result appropriately
            }
        }

    }
}
