using Bogus;
using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Interfaces.Users;
using Auth0.ManagementApi.Models;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Enums;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;
using Mapster;
using MapsterMapper;

namespace GigApp.Application.CQRS.Implementations.Auth.Commands;

public class RegisterAdmin
{
    public class Command : ICommand
    {
        public string Auth0UserId { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
    }

    public class Handler : ICommandHandler<Command>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        public Handler(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
        }
        public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _authRepository.SignUp(request.Email, request.Password);
            if (result.Status)
            {
                var data = (Auth0.ManagementApi.Models.User)result.Data;
                result = await _authRepository.AssignRole(data.UserId, "Admin");
                var user = new Domain.Entities.User
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Auth0UserId = data.UserId.Replace("auth0|", ""),
                };
                result = await _userRepository.AddUser(user, new List<UserType> { UserType.Admin });
                return result;
            }
            return result;
        }
    }
}
