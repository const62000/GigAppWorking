using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.Interfaces.Ratings;

public interface IRatingRepository
{
    Task<BaseResult> AddRatingAsync(Rating rating);
    Task<BaseResult> GetFacilityRatings(long id);
    Task<BaseResult> GetCaregiverRatings(long id);
    Task<BaseResult> GetCaregiverRatingByJobId(long caregiverId, long jobId);

}