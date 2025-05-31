using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Payments;
using GigApp.Application.Interfaces.Vendors;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Payments.Queries
{
    public class DeletePaymentMethod
    {
        public class Query : IQuery
        {
            public string Auth0Id {  get; set; } = string.Empty;
            public int PaymentMethodId { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.PaymentMethodId).NotEqual(0);
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
                var result = await _paymentRepository.DeletePaymentMethod(request.PaymentMethodId,request.Auth0Id);
                return result;
            }
        }
    }
}
