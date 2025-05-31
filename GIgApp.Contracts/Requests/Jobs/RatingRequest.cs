namespace GIgApp.Contracts.Requests.Jobs;

public record RatingRequest
{
    public int JobId { get; set; }
    public int JobHireId { get; set; }
    public int JobStarRating { get; set; }
}