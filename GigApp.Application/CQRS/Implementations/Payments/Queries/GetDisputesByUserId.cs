using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Payments;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Payments.Queries
{
    public class GetDisputesByUserId
    {
        public class Query : IQuery
        {
            public string Auth0Id { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Auth0Id).NotEmpty();
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
                var result = await _disputeRepository.GetDisputeByUserId(request.Auth0Id);
                return result;
            }
        }
    }
}
