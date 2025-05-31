using FluentValidation;
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
    public class DeleteJob
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
                var result = await _jobRepository.DeleteJobAsync(jobId);

                if (!result.Status)
                {
                    return new BaseResult(new {}, false, result.Message);
                }

                return new BaseResult(result.Data, true, "Job deleted successfully!");
            }
        }
    }
}
