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
    public class JobQuestionnaireAnswersByJobApplicationId
    {
        public class Query : IQuery
        {
            public long JobApplicationId { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.JobApplicationId).GreaterThan(0).WithMessage("Job Application Id must be a valid job ID");
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
                var jobApplicationId = request.JobApplicationId; 

                // Validate the request
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                try
                {
                    var jobQuestionAnswerData = await _jobQuestionRepository.GetQuestionaireAnswersByJobApplicationIdAsync(jobApplicationId);


                    if (jobQuestionAnswerData == null)
                    {
                        return new BaseResult(new { }, false, $"Job Application with ID {jobApplicationId} not found.");
                    }

                    return new BaseResult(jobQuestionAnswerData, true, "Job questions data retrieved successfully.");
                }
                catch (Exception ex)
                {
                    return new BaseResult(new {ex.Message }, false, "An error occurred while retrieving the job questions data. Please try again later.");
                }
            }
        }
    }
}
