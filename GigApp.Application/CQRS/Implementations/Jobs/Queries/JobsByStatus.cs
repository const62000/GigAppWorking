using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Responses.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Queries
{
    public class JobsByStatus
    {
        public class Query : IQuery
        {
            public string Status { get; set; } = string.Empty;
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Status).NotEmpty().WithMessage("Status must be a valid job status"); ;
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
                var status = request.Status;
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                var jobList = await _jobRepository.GetJobsByStatusAsync(status);

                var jobDetailResults = jobList.Select(job => new  JobStatusResult
                (
                    Id: job.Id,
                    Title: job.Title,
                    Status: job.Status
                )).ToList();
                // Check if job exists
                return new BaseResult(jobDetailResults, true, "Job list fetched successfully by status"); 


            }
        }
    }
}
