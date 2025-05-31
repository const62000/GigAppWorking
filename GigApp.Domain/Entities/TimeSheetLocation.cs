using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class TimeSheetLocation
{
    public long Id { get; set; }

    public long TimeSheetId { get; set; }

    public DateTime LocationCapturedDateTime { get; set; } = DateTime.UtcNow;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public virtual TimeSheet TimeSheet { get; set; } = null!;
}
