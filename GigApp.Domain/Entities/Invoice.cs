using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Invoice
{
    public int Id { get; set; }

    public int? ContractId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
