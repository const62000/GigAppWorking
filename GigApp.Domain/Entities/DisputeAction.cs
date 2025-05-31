using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class DisputeAction
{
    public long Id { get; set; }

    public long DisputeId { get; set; }

    public long ProcessedByUserId { get; set; }

    public DateTime ProcessedDateTime { get; set; }

    public string ActionMessage { get; set; } = null!;

    public string ActionStatus { get; set; } = null!;

    public virtual Dispute Dispute { get; set; } = null!;

    public virtual User ProcessedByUser { get; set; } = null!;
}
