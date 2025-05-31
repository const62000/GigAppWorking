using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Vendors;
using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GigApp.Application.Interfaces.Payments;

namespace GigApp.Application.CQRS.Implementations.Payments.Commands
{
    public class AddPaymentMethod
    {
        public class Command : ICommand
        {
            public string Auth0Id {  get; set; } = string.Empty;
            public string CardLastFour { get; set; } = null!;

            public string CardBrand { get; set; } = null!;

            public int ExpMonth { get; set; }

            public int ExpYear { get; set; }

            public string CardholderName { get; set; } = null!;

            public string StripePaymentMethodId { get; set; } = null!;

            public string StripeCardId { get; set; } = null!;

            public bool? IsDefault { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CardholderName).NotEmpty();
                RuleFor(x => x.Auth0Id).NotEmpty();
                RuleFor(x => x.CardLastFour).NotEmpty();
                RuleFor(x => x.ExpMonth).NotEqual(0);
                RuleFor(x => x.ExpYear).NotEqual(0);
                RuleFor(x => x.StripeCardId).NotEmpty();
                RuleFor(x => x.StripePaymentMethodId).NotEmpty();
                RuleFor(x => x.CardBrand).NotEmpty();
            }
        }

        internal sealed class Handler(IPaymentRepository _paymentRepository, IValidator<Command> _validator) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var paymentMethod = request.Adapt<PaymentMethod>();
                var reult = await _paymentRepository.AddPaymentMethod(paymentMethod, request.Auth0Id);
                return reult;
            }
        }
    }
}
