using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Dispute
{
    public long Id { get; set; }

    public string Type { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Attachment { get; set; }

    public int? HiredJobId { get; set; }

    public long? TimeSheetId { get; set; }

    public string Status { get; set; } = null!;

    public long? ProcessedByUserId { get; set; }

    public DateTime? ProcessedDateTime { get; set; }

    public virtual ICollection<DisputeAction> DisputeActions { get; set; } = new List<DisputeAction>();

    public virtual JobHire? HiredJob { get; set; }

    public virtual User? ProcessedByUser { get; set; }

    public virtual TimeSheet? TimeSheet { get; set; }
}
