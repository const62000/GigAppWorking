using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Users.Commands
{
    public class UpdateBankAccount
    {
        public class Command : ICommand
        {
            public int Id { get; set; }
            public string BankName { get; set; } = string.Empty;
            public string BankAccountType { get; set; } = string.Empty;
            public string BankAccountNumber { get; set; } = string.Empty;
            public string BankAccountName { get; set; } = string.Empty;
            public string BankSwiftCode { get; set; } = string.Empty;
            public string BankCountry { get; set; } = string.Empty;
            public string status { get; set; } = string.Empty;
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.BankAccountName).NotEmpty();
                RuleFor(x => x.BankAccountNumber).NotEmpty();
                RuleFor(x => x.BankName).NotEmpty();
                RuleFor(x => x.BankAccountType).NotEmpty();
                RuleFor(x => x.BankCountry).NotEmpty();
                RuleFor(x => x.BankSwiftCode).NotEmpty();
                RuleFor(x => x.Id).NotEqual(0);
            }
        }
        internal sealed class Handler(IValidator<Command> _validator, IBankAccountRepository _bankAccountRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var result = await _bankAccountRepository.UpdateBankAccount(request.Adapt<BankAccount>());
                return result;
            }
        }
    }
}
