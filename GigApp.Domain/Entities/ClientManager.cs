using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class ClientManager
{
    public int Id { get; set; }

    public long? UserId { get; set; }

    public long? ClientId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Client? Client { get; set; }

    public virtual User? User { get; set; }
}
