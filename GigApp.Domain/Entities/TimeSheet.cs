using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class TimeSheet
{
    public long Id { get; set; }

    public int HiringId { get; set; }

    public long UserId { get; set; }

    public DateTime ClockIn { get; set; } = DateTime.UtcNow;

    public DateTime? ClockOut { get; set; } = DateTime.UtcNow;

    public string? TimeSheetNotes { get; set; }

    public string Status { get; set; } = "Active";

    public string? TimeSheetApprovalStatus { get; set; }

    public virtual JobHire Hiring { get; set; } = null!;

    public virtual ICollection<TimeSheetLocation> TimeSheetLocations { get; set; } = new List<TimeSheetLocation>();

    public virtual User User { get; set; } = null!;
}
