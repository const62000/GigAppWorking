using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class CompanyLocation
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? AddressId { get; set; }

    public string LocationName { get; set; } = null!;

    public long? CompanyId { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual Address? Address { get; set; }

    public virtual Company? Company { get; set; }

    public virtual User? CreatedByUser { get; set; }
}
