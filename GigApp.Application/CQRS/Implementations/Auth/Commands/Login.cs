using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Interfaces.Notifications;
using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Auth.Commands
{
    public class Login
    {
        public class Query : IQuery
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string firebase_token { get; set; } = string.Empty;
            public string device_info { get; set; } = string.Empty;
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class Handler(IAuthRepository _authRepository, IValidator<Query> _validator ,IUserDeviceRepository _userDeviceRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var result = await _authRepository.Login(request.Email, request.Password);
                //await _notificationRepository.JobAppliedNotification(2, 1);
                if (!result.Status || string.IsNullOrEmpty(request.device_info) || string.IsNullOrEmpty(request.firebase_token)) 
                    return result;

                await _userDeviceRepository.AddUserDevice(new UserDevice { DeviceInfo = request.device_info,FirebaseToken = request.firebase_token},request.Email);
                return result;
            }
        }
    }
}
