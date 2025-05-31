using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Queries
{
    public class JobQuestionsById
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
            private readonly IJobQuestionRepository _jobQuestionRepository; 
            private readonly IValidator<Query> _validator;

            public Handler(IJobQuestionRepository jobQuestionRepository, IValidator<Query> validator)
            {
                _jobQuestionRepository = jobQuestionRepository;
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

                var jobQuestions = await _jobQuestionRepository.GetJobQuestionsById(jobId);

                if (jobQuestions == null)
                {
                    return new BaseResult(new { }, false, $"Job Application with ID {jobId} not found.");
                }

                // Return success result with job application data (if needed)
                return new BaseResult(new { jobQuestions }, true, "Job questions data retrieved successfully.");
            }

        }
    }
}
