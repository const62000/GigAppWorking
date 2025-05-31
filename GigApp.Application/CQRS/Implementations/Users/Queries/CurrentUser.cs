using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Users.Queries
{
    public class CurrentUser
    {
        public class Query : IQuery
        {
            public string Auth0Id { get; set; } = string.Empty;
        }

        public class IValidator : AbstractValidator<Query>
        {
            public IValidator()
            {
                RuleFor(x => x.Auth0Id).NotEmpty().WithMessage("Invalid token.");
            }
        }

        public class Handler : IQueryHandler<Query>
        {
            private readonly IUserRepository _userRepository;
            private readonly IValidator<Query> _validator;

            public Handler(IUserRepository userRepository, IValidator<Query> validator)
            {
                _userRepository = userRepository;
                _validator = validator;
            }
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var auth0Id = request.Auth0Id;
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid) 
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                try
                {
                    var result = await _userRepository.GetCurrentUser(auth0Id);

                    return result;
                }
                catch (Exception ex)
                {
                    return new BaseResult(new {ex.Message }, false, "An error occurred while fetching current user details.");
                }
            }
        }
        
    }
}
