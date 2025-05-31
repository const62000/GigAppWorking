using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Payments;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Payments.Queries
{
    public class GetDisputeById
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
        internal sealed class Handler(IDisputeRepository _disputeRepository, IValidator<Query> _validator) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var result = await _disputeRepository.GetDisputeById(request.Id);
                return result;
            }
        }
    }
}
