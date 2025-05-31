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
    public class GetHiringHistory
    {
        public class Query : IQuery
        {
            public long JobId { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator() 
            {
                RuleFor(x => x.JobId).NotEmpty();
            }
        }
        internal sealed class Handler(IHiringRepository _hiringRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _hiringRepository.GetHiringByJob(request.JobId);
                return result;
            }
        }
    }
}
