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
    public class GetTimeSheetByUserId
    {
        public class Query : IQuery
        {
            public string? Auth0Id { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Auth0Id).NotEmpty();
            }
        }
        internal sealed class Handler(ITimeSheetRepository _timeSheetRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _timeSheetRepository.GetTimeSheetByUserId(request.Auth0Id??string.Empty);
                return result;
            }
        }
    }
}
