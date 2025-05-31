using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class VendorsRegistrant
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public int VendorId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;

    public virtual Vendor Vendor { get; set; } = null!;
}
