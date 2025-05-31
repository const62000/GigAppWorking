using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class FreelancerLicense
{
    public int Id { get; set; }

    public long? FreelancerUserId { get; set; }

    public string? LicenseName { get; set; }

    public string? LicenseNumber { get; set; }

    public string IssuedBy { get; set; } = null!;

    public DateOnly IssuedDate { get; set; }

    public string LicenseStatus { get; set; } = "Active";

    public string? RejectionReason { get; set; }

    public string? LicenseFileType { get; set; }

    public string? LicenseFileUrl { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? FreelancerUser { get; set; }
}
