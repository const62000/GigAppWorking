using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Users.Commands
{
    public class UpdateUserProfile
    {
        public class Command : ICommand
        {
            public string ProfilePic { get; set; } = string.Empty;
            public string Auth0Id { get; set; } = string.Empty;
        }
        internal sealed class Handler(IUserRepository _userRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _userRepository.UpdateUserProfile(request.Auth0Id, request.ProfilePic);
                return result;
            }
        }
    }
}
