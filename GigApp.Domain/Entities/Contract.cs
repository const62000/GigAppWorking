using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Contract
{
    public int Id { get; set; }

    public int? VendorId { get; set; }

    public long? FacilityId { get; set; }

    public string Terms { get; set; } = null!;

    public decimal Pricing { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Company? Facility { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Vendor? Vendor { get; set; }
}
