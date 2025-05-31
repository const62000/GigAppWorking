using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class BankAccount
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public string BankName { get; set; } = null!;

    public string BankAccountType { get; set; } = null!;

    public string BankAccountNumber { get; set; } = null!;

    public string BankAccountName { get; set; } = null!;

    public string BankSwiftCode { get; set; } = null!;

    public string BankCountry { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    public string? StripeBankAccountId { get; set; }

    public string? StripeCustomerId { get; set; }

    public virtual User User { get; set; } = null!;
}
