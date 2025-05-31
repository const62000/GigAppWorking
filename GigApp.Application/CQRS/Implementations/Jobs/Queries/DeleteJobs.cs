using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Queries
{
    public class DeleteJobs
    {
        public class Query : IQuery
        {
            public List<long> Ids { get; set; } = new List<long>();
        }
        private bool HaveMoreThanOneItem(List<string> items)
        {
            // Check if the list is not null and has more than one item
            return items != null && items.Count > 1;
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Ids).NotNull().WithMessage(Messages.List_Empty).Must(x => x.Count > 0).WithMessage(Messages.List_Empty);
            }
        }
        internal sealed class Handler(IJobRepository _jobRepository, IValidator<Query> _validator) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                var result = await _jobRepository.DeleteJobsAsync(request.Ids);
                
                if(result == null)
                {
                    return new BaseResult(new { }, false, $"Jobs not found.");
                }
                return new BaseResult(result, true, "Jobs deleted successfully.");
            }
        }
    }
}
