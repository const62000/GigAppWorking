using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Jobs.Queries
{
    public class GetJobApplication
    {
        public class Query : IQuery
        {
            public long JobId { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.JobId).GreaterThan(0).WithMessage("JobId must be a valid job ID");
            }
        }
        public class Handler : IQueryHandler<Query>
        {
            private readonly IJobApplicationRepository _jobApplicationRepository;
            private readonly IValidator<Query> _validator;

            public Handler(IJobApplicationRepository jobApplicationRepository, IValidator<Query> validator)
            {
                _jobApplicationRepository = jobApplicationRepository;
                _validator = validator;
            }
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var jobId = request.JobId;

                // Validate the request
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                var getjobApplicationData = await _jobApplicationRepository.GetJobApplicationById(jobId);

                if (getjobApplicationData == null)
                {
                    return new BaseResult(new { }, false, $"Job Application with ID {jobId} not found.");
                }

                // Create job application result
                //var jobApplicationData = new List<JobApplicationResult>();
                //getjobApplicationData.ForEach(x => jobApplicationData.Add(new JobApplicationResult(x.Id,
                //    (long)x.FreelancerUserId,
                //    x.JobApplicationStatus)));



                // Return success result with job application data (if needed)
                return new BaseResult(new { getjobApplicationData }, true, "Job application data retrieved successfully.");
            }

        }
    }
}
