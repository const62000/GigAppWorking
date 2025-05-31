using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Payments;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Payments.Queries;

public class GetPayments 
{
    public class Query : IQuery
    {
        public string Auth0Id { get; set; } = string.Empty;
        public long? JobId { get; set; }
    }
    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Auth0Id).NotEmpty();
        }
    }
    internal sealed class Handler(IPaymentRepository _paymentRepository, IValidator<Query> _validator) : IQueryHandler<Query>
    {
        public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
            }
            var result = await _paymentRepository.GetPayments(request.Auth0Id, request.JobId);
            return result;
        }
    }
}