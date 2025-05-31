using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GigApp.Application.Interfaces.Ratings;
using Microsoft.EntityFrameworkCore;

namespace GigApp.Infrastructure.Repositories.Ratings;

public class RatingRepository(ApplicationDbContext _context) : IRatingRepository
{
    public async Task<BaseResult> AddRatingAsync(Rating rating)
    {
        var hire = await _context.JobHires.FirstOrDefaultAsync(r => r.Id == rating.JobHireId);
        if (hire == null)
        {
            return new BaseResult(new { }, false, "Job Hire not found");
        }
        if (hire.Status != "Completed")
        {
            return new BaseResult(new { }, false, "Job is not completed");
        }
        await _context.Ratings.AddAsync(rating);
        await _context.SaveChangesAsync();
        return new BaseResult(new { rating.Id }, true, string.Empty);
    }

    public async Task<BaseResult> GetFacilityRatings(long id)
    {
        var ratings = await _context.Ratings.Where(r => r.FacilityId == id).OrderByDescending(r => r.CreatedAt).IncludeFacilityRatingDetails().ToListAsync();
        if (ratings.Count == 0)
        {
            return new BaseResult(new { }, false, "No ratings found");
        }
        var averageRating = ratings.Average(r => r.FacilityStarRating);
        return new BaseResult(new { ratings, averageRating }, true, string.Empty);
    }

    public async Task<BaseResult> GetCaregiverRatings(long id)
    {
        var ratings = await _context.Ratings.Where(r => r.CaregiverId == id).OrderByDescending(r => r.CreatedAt).IncludeCaregiverRatingDetails().ToListAsync();
        if (ratings.Count == 0)
        {
            return new BaseResult(new { }, false, "No ratings found");
        }
        var averageRating = ratings.Average(r => r.CaregiverStarRating);
        return new BaseResult(new { ratings, averageRating }, true, string.Empty);
    }

    public async Task<BaseResult> GetCaregiverRatingByJobId(long caregiverId, long jobId)
    {
        var rating = await _context.Ratings.IncludeCaregiverRatingDetails().FirstOrDefaultAsync(r => r.CaregiverId == caregiverId && r.JobId == jobId);
        if (rating == null)
        {
            return new BaseResult(new { }, false, "Rating not found");
        }
        return new BaseResult(rating, true, string.Empty);
    }
}