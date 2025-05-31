using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Payments;
using GIgApp.Contracts.Responses;
using Stripe;

namespace GigApp.Application.CQRS.Implementations.Payments.Queries
{
    public class StripeWebHooks
    {
        public class Query : IQuery
        {
            public Event Event { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Event).NotEmpty();
            }
        }
        internal sealed class Handler(IValidator<Query> _validator,IStripePaymentRepository _stripePaymentRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var result = await _stripePaymentRepository.NotifySenderAndReceiver(request.Event);
                return result;
            }
        }
    }
}
