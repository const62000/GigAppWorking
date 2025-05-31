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
using System;
using System.Collections.ObjectModel;
using static GigApp.Application.CQRS.Implementations.Auth.Commands.SignUp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GigApp.Application.CQRS.Implementations.Auth.Commands
{
    public class SignUp
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<LicenseRequest, FreelancerLicense>()
                .Map(des => des.LicenseName, src => src.LicenseName)
                .Map(des => des.LicenseNumber, src => src.LicensesNumber)
                .Map(des => des.LicenseFileUrl, src => src.FileUrl)
                .Map(des => des.IssuedBy, src => src.IssuedBy);

            config.NewConfig<Command, GigApp.Domain.Entities.User>()
                .Map(des => des.FirstName, src => src.FirstName)
                .Map(des => des.LastName, src => src.LastName)
                .Map(des => des.Email, src => src.Email)
                .Map(des => des.Auth0UserId, src => src.Auth0UserId);

        }

        public class Command : ICommand
        {
            public string Auth0UserId { get; set; } = Guid.NewGuid().ToString();
            public string Email { get; set; } = null!;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public required string Password { get; set; } = string.Empty;
            public List<UserType> UserTypes { get; set; } = new List<UserType>();
            public AddressRequest Address { get; set; } = new AddressRequest(string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, string.Empty);
            public List<LicenseRequest> Licenses { get; set; } = new List<LicenseRequest>();
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Licenses).NotNull();
            }
        }
        public class Handler(IAuthRepository _authRepository, IUserRepository _userRepository, IValidator<Command> _validator, IMapper _mapper) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var user = _mapper.Map<GigApp.Domain.Entities.User>(request);
                if (request.Address != null)
                    user.Addresses.Add(_mapper.Map<Address>(request.Address));
                foreach (var item in request.Licenses)
                {
                    var data = _mapper.Map<FreelancerLicense>(item);
                    data.LicenseFileUrl = item.FileUrl;
                    var date = DateOnly.FromDateTime(item.DateOfIssue);
                    data.IssuedDate = date;
                    user.FreelancerLicenses.Add(data);
                }
                var userResponse = await _userRepository.AddUser(user, request.UserTypes);

                if (!userResponse.Status)
                {
                    return new BaseResult(userResponse, false, string.Empty);
                }

                var result = await _authRepository.SignUp(request.Email, request.Password);
                long userId = (long)userResponse.Data;
                if (result.Status)
                {
                    var data = (Auth0.ManagementApi.Models.User)result.Data;
                    if (data != null)
                    {
                        string auth0Id = string.IsNullOrEmpty(data.UserId) ? string.Empty : data.UserId.Replace("auth0|", "");
                        result = await _userRepository.AddAuth0Id(userId, auth0Id);
                    }
                }
                else
                {
                    var deleteResult = await _userRepository.DeleteUser(userId);
                    return result;
                }
                return result;
            }
        }
    }
}
