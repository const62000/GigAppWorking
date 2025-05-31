using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class UserDevice
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public string FirebaseToken { get; set; } = null!;

    public string DeviceInfo { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
}
