using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class UserVendor
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int VendorId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Vendor Vendor { get; set; } = null!;
}
