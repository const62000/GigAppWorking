using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class JobDetails
    {
        public class Query : IQuery
        {
            public long JobId { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.JobId).GreaterThan(0).WithMessage("JobId must be a valid/positive number.");
            }
        }
        public class Handler : IQueryHandler<Query>
        {
            private readonly IJobRepository _jobRepository;
            private readonly IValidator<Query> _validator;

            public Handler(IJobRepository jobRepository, IValidator<Query> validator)
            {
                _jobRepository = jobRepository;
                _validator = validator;
            }

            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var jobId = request.JobId;
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                var jobDetails = await _jobRepository.GetJobDetailsAsync(jobId);

                // Check if job exists
                if (jobDetails == null)
                {
                    return new BaseResult(new { }, false, $"Job with ID {jobId} not found.");
                }

                //var jobDetailResult = new JobDetailsResult
                //(
                //    Id : jobDetails.Id,
                //    Title : jobDetails.Title,
                //    Description : jobDetails.Description,
                //    Requirements : jobDetails.Requirements,
                //    Date: jobDetails.Date,
                //    Time : jobDetails.Time,
                //    Rate : jobDetails.Rate,
                //    Status : jobDetails.Status
                //);

                return new BaseResult(jobDetails , true, "Job details fetched successfully!");
            }
        }
    }
}
