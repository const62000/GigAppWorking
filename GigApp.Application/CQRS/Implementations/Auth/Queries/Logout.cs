using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Auth.Queries
{
    public class Logout
    {
        public class Query : IQuery
        {
            public string Id { get; set; } = string.Empty;
        }
        public class IValidator : AbstractValidator<Query> 
        {
            public IValidator() 
            {
                RuleFor(x=>x.Id).NotEmpty().WithMessage(Messages.Logout_Fail);
            }
        }
        internal sealed class Handler(IUserDeviceRepository _userDeviceRepository , IUserRepository _userRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var userId =await _userRepository.GetUserId(request.Id);
                var result = await _userDeviceRepository.DeleteUserDevice(userId);
                return result;
            }
        }
    }
}
