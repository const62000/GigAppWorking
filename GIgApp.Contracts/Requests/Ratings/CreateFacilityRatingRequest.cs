namespace GIgApp.Contracts.Requests.Ratings;

public record CreateFacilityRatingRequest(
    long JobId,
    int JobHireId,
    long FacilityId,
    int FacilityStarRating,
    string? Feedback
);