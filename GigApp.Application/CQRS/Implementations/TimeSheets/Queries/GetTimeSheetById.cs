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
    public class GetTimeSheetById
    {
        public class Query : IQuery
        {
            public long Id { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEqual(0);
            }
        }
        internal sealed class Handler(ITimeSheetRepository _timeSheetRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _timeSheetRepository.GetTimeSheetById(request.Id);
                return result;
            }
        }
    }
}
