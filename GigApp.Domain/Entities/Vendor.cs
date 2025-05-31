using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigApp.Domain.Entities;

public partial class Vendor
{
    public int Id { get; set; }

    public long? UserId { get; set; }

    public string? Name { get; set; }

    public string? ServicesOffered { get; set; }

    public string? Certifications { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual User? User { get; set; }

    public virtual ICollection<UserVendor> UserVendors { get; set; } = new List<UserVendor>();

    public virtual ICollection<VendorsRegistrant> VendorsRegistrants { get; set; } = new List<VendorsRegistrant>();
    [NotMapped]
    public string? ManagerName { get; set; }
}
