using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Ratings;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Ratings.Queries;

public class GetCaregiveRatingByJobId
{
    public class Query : IQuery
    {
        public long CaregiverId { get; set; }
        public long JobId { get; set; }
    }

    internal sealed class Handler : IQueryHandler<Query>
    {
        private readonly IRatingRepository _ratingRepository;

        public Handler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _ratingRepository.GetCaregiverRatingByJobId(request.CaregiverId, request.JobId);
            return result;
        }
    }
}