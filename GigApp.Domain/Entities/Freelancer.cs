using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Freelancer
{
    public int UserId { get; set; }

    public long? AddressId { get; set; }

    public string? LicenseInfo { get; set; }

    public string? BankAccountInfo { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Address? Address { get; set; }
}
