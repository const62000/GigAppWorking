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
    public class GetHiredJobByHiredManager
    {
       public class Query : IQuery
        {
            public string Auth0Id { get; set; } = string.Empty;
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator() 
            {
                RuleFor(x => x.Auth0Id).NotEmpty();
            }
        }
        internal sealed class Handler(IHiringRepository _hiringRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _hiringRepository.GetHiringByManager(request.Auth0Id);
                return result;
            }
        } 
    }
}