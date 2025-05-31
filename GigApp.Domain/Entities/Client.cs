using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigApp.Domain.Entities;

public partial class Client
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? AdminUserId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual User? AdminUser { get; set; }

    public virtual ICollection<ClientLocation> ClientLocations { get; set; } = new List<ClientLocation>();

    public virtual ICollection<ClientManager> ClientManagers { get; set; } = new List<ClientManager>();

    public virtual ICollection<FacilityManager> FacilityManagers { get; set; } = new List<FacilityManager>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    [NotMapped]

    public int LocationsCount { get; set; }
}
