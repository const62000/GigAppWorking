using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Ratings;
using GIgApp.Contracts.Requests.Jobs;
using Mapster;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands;

public class AddRating
{
    public class Command : ICommand
    {
        public int JobId { get; set; }
        public int JobHireId { get; set; }
        public int JobStarRating { get; set; }
        public string Auth0Id { get; set; }
    }

    public class Handler(IRatingRepository ratingRepository) : ICommandHandler<Command>
    {
        public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var rating = request.Adapt<Rating>();
            var result = await ratingRepository.AddRatingAsync(rating);
            return result;
        }
    }
}