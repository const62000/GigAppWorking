using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class Address
{
    public long Id { get; set; }

    public string? Label { get; set; }

    public string StreetAddress1 { get; set; } = null!;

    public string? StreetAddress2 { get; set; }

    public string City { get; set; } = null!;

    public string? StateProvince { get; set; }

    public string? PostalCode { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string Country { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public long? ClientId { get; set; }

    public long? UserId { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<ClientLocation> ClientLocations { get; set; } = new List<ClientLocation>();

    public virtual ICollection<Freelancer> Freelancers { get; set; } = new List<Freelancer>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual User? User { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
