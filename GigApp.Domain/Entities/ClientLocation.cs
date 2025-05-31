using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class ClientLocation
{
    public long Id { get; set; }

    public long? AddressId { get; set; }

    public string LocationName { get; set; } = null!;

    public long? ClientId { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual Address? Address { get; set; }

    public virtual Client? Client { get; set; }

    public virtual User? CreatedByUser { get; set; }
}
