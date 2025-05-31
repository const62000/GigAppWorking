using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class FacilityManager
{
    public int Id { get; set; }

    public long? UserId { get; set; }

    public long? FacilityId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Client? Facility { get; set; }

    public virtual User? User { get; set; }
}
