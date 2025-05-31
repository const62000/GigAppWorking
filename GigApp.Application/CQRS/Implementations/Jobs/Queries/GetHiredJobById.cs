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
    public class GetHiredJobById
    {
       public class Query : IQuery
        {
            public int Id { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator() 
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }
        internal sealed class Handler(IHiringRepository _hiringRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _hiringRepository.GetHiredJobById(request.Id);
                return result;
            }
        } 
    }
}