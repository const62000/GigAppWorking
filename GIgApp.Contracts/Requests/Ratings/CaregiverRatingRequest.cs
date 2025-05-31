using System.ComponentModel.DataAnnotations;

namespace GIgApp.Contracts.Requests.Ratings;

public record CaregiverRatingRequest
{
    public long JobId { get; set; }
    public int JobHireId { get; set; }
    public long CaregiverId { get; set; }
    [Range(1, 5)]
    public int CaregiverStarRating { get; set; }
    public string? Feedback { get; set; }
}
