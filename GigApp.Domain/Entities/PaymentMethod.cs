using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public long? ClientId { get; set; }

    public string CardLastFour { get; set; } = null!;

    public string CardBrand { get; set; } = null!;

    public int ExpMonth { get; set; }

    public int ExpYear { get; set; }

    public string CardholderName { get; set; } = null!;

    public string StripePaymentMethodId { get; set; } = null!;

    public string StripeCardId { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public string? StripeCustomerId { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User User { get; set; } = null!;
}
