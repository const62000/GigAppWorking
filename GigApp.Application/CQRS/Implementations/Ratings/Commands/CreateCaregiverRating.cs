using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Ratings;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using Mapster;

namespace GigApp.Application.CQRS.Implementations.Ratings.Commands
{
    public class CreateCaregiverRating
    {
        public class Command : ICommand
        {
            public long JobId { get; set; }
            public int JobHireId { get; set; }
            public long CaregiverId { get; set; }
            public int CaregiverStarRating { get; set; }
            public string? Feedback { get; set; }
            public string Auth0Id { get; set; } = string.Empty;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CaregiverStarRating).InclusiveBetween(1, 5)
                    .WithMessage("Rating must be between 1 and 5 stars");
                RuleFor(x => x.JobId).GreaterThan(0);
                RuleFor(x => x.JobHireId).GreaterThan(0);
                RuleFor(x => x.CaregiverId).GreaterThan(0);
                RuleFor(x => x.Auth0Id).NotEmpty();
            }
        }

        internal sealed class Handler : ICommandHandler<Command>
        {
            private readonly IRatingRepository _ratingRepository;
            private readonly IValidator<Command> _validator;
            private readonly IUserRepository _userRepository;


            public Handler(IRatingRepository ratingRepository, IValidator<Command> validator, IUserRepository userRepository)
            {
                _ratingRepository = ratingRepository;
                _validator = validator;
                _userRepository = userRepository;
            }

            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }

                var userId = await _userRepository.GetUserId(request.Auth0Id);
                if (userId == 0)
                {
                    return new BaseResult(new { }, false, "User not found");
                }

                var rating = request.Adapt<Rating>();
                rating.RatingGivenByUserId = userId;
                var result = await _ratingRepository.AddRatingAsync(rating);
                return result;
            }
        }
    }
}