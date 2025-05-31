using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Job
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Requirements { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public decimal Rate { get; set; }

    public string Status { get; set; } = null!;

    public long? JobManagerUserId { get; set; }

    public long? AddressId { get; set; }

    public long? ClientId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public string? LicenseRequirments { get; set; }

    public int? HoursPerWeek { get; set; }

    public int? JobType { get; set; }

    public string? PaymentIntentId { get; set; }

    public int? PaymentMethodId { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? EndedDate { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual ICollection<JobHire> JobHires { get; set; } = new List<JobHire>();

    public virtual User? JobManagerUser { get; set; }

    public virtual ICollection<JobQuestionnaire> JobQuestionnaires { get; set; } = new List<JobQuestionnaire>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
