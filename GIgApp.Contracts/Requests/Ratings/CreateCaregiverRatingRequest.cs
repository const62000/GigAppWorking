public record CreateCaregiverRatingRequest(
    long JobId,
    int JobHireId,
    long CaregiverId,
    int CaregiverStarRating
// string? Feedback
);