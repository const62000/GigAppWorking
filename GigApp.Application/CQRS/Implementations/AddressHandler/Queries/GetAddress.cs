using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Addresses;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.AddressHandler.Queries
{
    public class GetAddress
    {
        public class Query : IQuery
        {
            public string Id { get; set; } = string.Empty;
        }
        public class IValidator : AbstractValidator<Query>
        {
            public IValidator()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage(Messages.Logout_Fail);
            }
        }
        public class Handler(IAddressesRepository _addressRepository, IUserRepository _userRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var userId = await _userRepository.GetUserId(request.Id);
                
                try
                {
                    var result = await _addressRepository.GetAddressAsync(userId);
                    return result;
                }
                catch (Exception ex)
                {
                    return new BaseResult(new {ex.Message }, false, "An error occurred while retrieving the job questions data. Please try again later.");
                }
            }
        }
    }
}
