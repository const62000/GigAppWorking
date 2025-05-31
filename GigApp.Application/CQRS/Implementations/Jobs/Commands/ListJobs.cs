using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.CQRS.Abstractions.Query;
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
    public class ListJobs
    {
        public class Command : ICommand
        {
            public int Page { get; set; }
            public int PerPage { get; set; }
            public string SearchQuery { get; set; } = string.Empty;
            public Dictionary<string, string> Filters { get; set; } = new();
            public string SortBy { get; set; }
            public bool Ascending { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public decimal Miles { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be a positive integer.");

                RuleFor(x => x.PerPage)
                    .GreaterThan(0)
                    .WithMessage("PerPage must be a positive integer.");
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
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                // Map Command to JobSearchRequest
                var jobSearchRequest = new JobSearchRequest(
                    request.Page,
                    request.PerPage,
                    request.SearchQuery,
                    request.Filters,
                    request.SortBy,
                    request.Ascending,
                    request.Latitude,
                    request.Longitude,  
                    request.Miles
                    );

                var jobList = await _jobRepository.ListJobsAsync(jobSearchRequest);

                //var jobDetailResults = jobList.Select(job => new
                //{
                //    Id= job.Id,
                //    Title= job.Title,
                //    Description= job.Description,
                //    Requirements= job.Requirements,
                //    Date= job.Date,
                //    Time= job.Time,
                //    Rate= job.Rate,
                //    Status= job.Status,
                //    JobManager = job.JobManagerUser,
                //    Address = job.Address,
                //    Facility = job.Facility

                //}).ToList();

                return new BaseResult(jobList, true, "Job details list fetched successfully!");
            }

        }
    }
}
