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
using GigApp.Application.Interfaces.Users;

namespace GigApp.Application.CQRS.Implementations.Payments.Commands;

public class AddStripeConnect 
{
    public class Command : ICommand
    {
        public string Auth0Id { get; set; } = string.Empty;
    }   

    public class Handler(IStripePaymentRepository _stripeRepository, IUserRepository _userRepository) : ICommandHandler<Command>
    {
        public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetCurrentUser(request.Auth0Id);
            if (!result.Status)
                return result;
            var user = result.Data as User;
            if (user == null)
                return new BaseResult(null, false, "User not found");
            result = await _stripeRepository.CreateStripeAccount(user.Email);
            if (!result.Status)
                return result;
            var accountId = result.Data as string;
            var updateResult = await _userRepository.UpdateUserStripeAccountId(user.Id, accountId);
            if (!updateResult.Status)
                return updateResult;
            var linkResult = await _stripeRepository.CreateStripeAccountLink(accountId);
            if (!linkResult.Status)
                return linkResult;
            var link = linkResult.Data as string;
            return new BaseResult(link, true, result.Message);
        }
    }
}