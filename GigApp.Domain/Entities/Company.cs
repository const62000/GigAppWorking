using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Company
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? AdminUserId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual User? AdminUser { get; set; }

    public virtual ICollection<CompanyLocation> CompanyLocations { get; set; } = new List<CompanyLocation>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<FacilityManager> FacilityManagers { get; set; } = new List<FacilityManager>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
