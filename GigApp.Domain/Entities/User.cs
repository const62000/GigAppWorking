using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigApp.Domain.Entities;

public partial class User
{
    public long Id { get; set; }

    public string? Auth0UserId { get; set; }

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool Disabled { get; set; }

    public long? DisabledByUserId { get; set; }

    public long? AddressId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public string? Picture { get; set; }

    public string? StripeAccountId { get; set; }

    public string? StripeCustomerId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    public virtual ICollection<ClientLocation> ClientLocations { get; set; } = new List<ClientLocation>();

    public virtual ICollection<ClientManager> ClientManagers { get; set; } = new List<ClientManager>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual User? DisabledByUser { get; set; }

    public virtual ICollection<FacilityManager> FacilityManagers { get; set; } = new List<FacilityManager>();

    public virtual ICollection<FreelancerLicense> FreelancerLicenses { get; set; } = new List<FreelancerLicense>();

    public virtual ICollection<User> InverseDisabledByUser { get; set; } = new List<User>();

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual ICollection<JobHire> JobHireFreelancers { get; set; } = new List<JobHire>();

    public virtual ICollection<JobHire> JobHireHiredManagers { get; set; } = new List<JobHire>();

    public virtual ICollection<JobQuestionnaireAnswer> JobQuestionnaireAnswers { get; set; } = new List<JobQuestionnaireAnswer>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    public virtual ICollection<Payment> PaymentPaidByUsers { get; set; } = new List<Payment>();

    public virtual ICollection<Payment> PaymentPaidToUsers { get; set; } = new List<Payment>();

    public virtual ICollection<Rating> RatingCaregivers { get; set; } = new List<Rating>();

    public virtual ICollection<Rating> RatingJobManagers { get; set; } = new List<Rating>();

    public virtual ICollection<Rating> RatingRatingGivenByUsers { get; set; } = new List<Rating>();

    public virtual ICollection<TimeSheet> TimeSheets { get; set; } = new List<TimeSheet>();

    public virtual ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();

    public virtual ICollection<UserVendor> UserVendors { get; set; } = new List<UserVendor>();

    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();

    public virtual ICollection<VendorsRegistrant> VendorsRegistrants { get; set; } = new List<VendorsRegistrant>();

    [NotMapped]
    public string? VendorName { get; set; }
}
