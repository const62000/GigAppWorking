using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Rating
{
    public int Id { get; set; }

    public long RatingGivenByUserId { get; set; }

    public long? JobId { get; set; }

    public int? JobHireId { get; set; }

    public int? JobStarRating { get; set; }

    public long? CaregiverId { get; set; }

    public int? CaregiverStarRating { get; set; }

    public long? FacilityId { get; set; }

    public string? Feedback { get; set; }

    public int? FacilityStarRating { get; set; }

    public long? JobManagerId { get; set; }

    public int? JobManagerStarRating { get; set; }

    public int? VendorId { get; set; }

    public int? VendorStarRating { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? Caregiver { get; set; }

    public virtual Client? Facility { get; set; }

    public virtual Job? Job { get; set; }

    public virtual JobHire? JobHire { get; set; }

    public virtual User? JobManager { get; set; }

    public virtual User RatingGivenByUser { get; set; } = null!;

    public virtual Vendor? Vendor { get; set; }
}
