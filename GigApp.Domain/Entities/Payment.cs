using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public long? JobId { get; set; }

    public int? PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public string PaymentType { get; set; } = null!;

    public string? EscrowStatus { get; set; }

    public string? StripePaymentIntentId { get; set; }

    public string? StripeTransferId { get; set; }

    public string? Description { get; set; }

    public string? FailureReason { get; set; }

    public long? PaidByUserId { get; set; }

    public long? PaidToUserId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual Job? Job { get; set; }

    public virtual User? PaidByUser { get; set; }

    public virtual User? PaidToUser { get; set; }

    public virtual PaymentMethod? PaymentMethod { get; set; }
}
