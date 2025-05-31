using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Mapster;
using GigApp.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Users.Commands
{
    public class AddBankAccount
    {
        public class Command : ICommand
        {
            public string Auth0Id { get; set; } = string.Empty;
            public string BankName { get; set; } = string.Empty;
            public string BankAccountType { get; set; } = string.Empty;
            public string BankAccountNumber { get; set; } = string.Empty;
            public string BankAccountName { get; set; } = string.Empty;
            public string BankSwiftCode { get; set; } = string.Empty;
            public string BankCountry { get; set; } = string.Empty;
            public string status { get; set; } 
            public string BankToken { get; set; } = string.Empty;
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.BankAccountName).NotEmpty();
                RuleFor(x=>x.BankAccountNumber).NotEmpty();
                RuleFor(x=>x.BankName).NotEmpty();
                RuleFor(x=>x.BankAccountType).NotEmpty();
                RuleFor(x=>x.BankCountry).NotEmpty();
                RuleFor(x=>x.BankSwiftCode).NotEmpty();
                RuleFor(x=>x.Auth0Id).NotEmpty();
            }
        }
        internal sealed class Handler(IValidator<Command> _validator,IBankAccountRepository _bankAccountRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                request.status =  BankAccountStatus.Pending.ToString();
                var result = await _bankAccountRepository.AddBankAccount(request.Adapt<BankAccount>(), request.Auth0Id,request.BankToken);
                return result;
            }
        }
    }
}
