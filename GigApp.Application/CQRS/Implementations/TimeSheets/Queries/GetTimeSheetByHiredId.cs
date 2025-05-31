using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.TimeSheets.Queries
{
    public class GetTimeSheetByHiredId
    {
        public class Query : IQuery
        {
            public int HiredId { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.HiredId).NotEqual(0);
            }
        }
        internal sealed class Handler(ITimeSheetRepository _timeSheetRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _timeSheetRepository.GetTimeSheetByHiredId(request.HiredId);
                return result;
            }
        }
    }
}
