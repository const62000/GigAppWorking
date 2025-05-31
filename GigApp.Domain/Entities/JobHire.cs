using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class JobHire
{
    public int Id { get; set; }

    public long JobId { get; set; }

    public long? FreelancerId { get; set; }

    public long HiredManagerId { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? Freelancer { get; set; }

    public virtual User HiredManager { get; set; } = null!;

    public virtual Job Job { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<TimeSheet> TimeSheets { get; set; } = new List<TimeSheet>();
}
