using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Type { get; set; }

    public DateTime NotificationTime { get; set; } = DateTime.UtcNow;

    public bool HasViewed { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
}
