using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Notifications;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using GigApp.Contracts.Enums;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class CreateJob
    {
        public class Command : ICommand
        {
            public string Auth0Id { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public string? Requirements { get; set; }
            public string? LicenseRequirments {  get; set; }
            public DateOnly Date { get; set; }

            public TimeOnly Time { get; set; }

            public decimal Rate { get; set; }

            public string Status { get; set; } = string.Empty;
            public long AddressId { get; set; }
            public long FacilityId {  get; set; }

            public JobType JobType { get; set; }
    
            public int HoursPerWeek { get; set; }

            public int PaymentMethodId { get; set; }

            public DateTime StartedDate { get; set; }
            public DateTime EndedDate { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Auth0Id).NotEmpty();
                RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required."); 
                RuleFor(x => x.Rate).GreaterThan(0).WithMessage("Rate must be a positive number.");
                RuleFor(x => x.AddressId).NotEqual(0).WithMessage("You should select an address");
                RuleFor(x => x.PaymentMethodId).NotEqual(0).WithMessage("You should select a payment method");
                RuleFor(x => x.StartedDate).NotNull().WithMessage("Started date is required");
                RuleFor(x => x.EndedDate).NotNull().WithMessage("Ended date is required");
            }
        }
        public class Handler : ICommandHandler<Command>
        {
            private readonly IJobRepository _jobRepository;
            private readonly IValidator<Command> _validator;
            private readonly INotificationRepository _notificationRepository;

            public Handler(IJobRepository jobRepository, IValidator<Command> validator,INotificationRepository notificationRepository)
            {
                _jobRepository = jobRepository;
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
                var createJobRequest = new CreateJobRequest(
                    request.Title,
                    request.Description,
                    request.LicenseRequirments,
                    request.Requirements,
                    request.Date,
                    request.Time,
                    request.Rate,
                    request.Status,
                    request.AddressId,
                    request.FacilityId,
                    request.JobType,
                    request.HoursPerWeek,
                    request.PaymentMethodId,
                    request.StartedDate,
                    request.EndedDate
                );

                
                var createJobResult = await _jobRepository.CreateJobAsync(createJobRequest, request.Auth0Id);
                if(createJobResult.jobId == 0)
                    return new BaseResult(new {},false, createJobResult.message);
                await _notificationRepository.JobCreatedNotification(createJobResult.jobId,request.Auth0Id);
                return new BaseResult(new { JobId = createJobResult.jobId }, true, "Job created successfully!"); // Make sure to return the result appropriately
            } 
        }

    }
}
