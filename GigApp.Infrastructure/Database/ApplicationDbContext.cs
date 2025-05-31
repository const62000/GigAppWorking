using System;
using System.Collections.Generic;
using GigApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GigApp.Infrastructure.Database;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientLocation> ClientLocations { get; set; }

    public virtual DbSet<ClientManager> ClientManagers { get; set; }

    public virtual DbSet<FacilityManager> FacilityManagers { get; set; }

    public virtual DbSet<Freelancer> Freelancers { get; set; }

    public virtual DbSet<FreelancerLicense> FreelancerLicenses { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobApplication> JobApplications { get; set; }

    public virtual DbSet<JobHire> JobHires { get; set; }

    public virtual DbSet<JobQuestionnaire> JobQuestionnaires { get; set; }

    public virtual DbSet<JobQuestionnaireAnswer> JobQuestionnaireAnswers { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<TimeSheet> TimeSheets { get; set; }

    public virtual DbSet<TimeSheetLocation> TimeSheetLocations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDevice> UserDevices { get; set; }

    public virtual DbSet<UserVendor> UserVendors { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    public virtual DbSet<VendorsRegistrant> VendorsRegistrants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC07EC7BF17F");

            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Label).HasMaxLength(255);
            entity.Property(e => e.Latitude).HasColumnType("decimal(10, 7)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(10, 7)");
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.StateProvince).HasMaxLength(100);
            entity.Property(e => e.StreetAddress1).HasMaxLength(255);
            entity.Property(e => e.StreetAddress2).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Client).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Addresses_Client");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Addresses_User");
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.StripeBankAccountId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StripeCustomerId)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.BankAccounts).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC071B883F21");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.AdminUser).WithMany(p => p.Clients)
                .HasForeignKey(d => d.AdminUserId)
                .HasConstraintName("FK_Clients_AdminUser");
        });

        modelBuilder.Entity<ClientLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientLo__3214EC074F7794EA");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LocationName).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.ClientLocations)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_ClientLocations_Address");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientLocations)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_ClientLocations_Client");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.ClientLocations)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_ClientLocations_CreatedByUser");
        });

        modelBuilder.Entity<ClientManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientMa__3214EC07472DDA13");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientManagers)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__ClientMan__Clien__2EFAF1E2");

            entity.HasOne(d => d.User).WithMany(p => p.ClientManagers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ClientMan__UserI__2FEF161B");
        });

        modelBuilder.Entity<FacilityManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facility__3214EC07D28F4989");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Facility).WithMany(p => p.FacilityManagers)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__FacilityM__Facil__0F824689");

            entity.HasOne(d => d.User).WithMany(p => p.FacilityManagers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FacilityM__UserI__10766AC2");
        });

        modelBuilder.Entity<Freelancer>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Freelanc__1788CC4C96B1389C");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.Freelancers)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__Freelance__Addre__125EB334");
        });

        modelBuilder.Entity<FreelancerLicense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Freelanc__3214EC076FE22C2E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IssuedBy).HasMaxLength(100);
            entity.Property(e => e.LicenseFileType).HasMaxLength(20);
            entity.Property(e => e.LicenseStatus).HasMaxLength(20);

            entity.HasOne(d => d.FreelancerUser).WithMany(p => p.FreelancerLicenses)
                .HasForeignKey(d => d.FreelancerUserId)
                .HasConstraintName("FK__Freelance__Freel__116A8EFB");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoices__3214EC07D76C8642");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Jobs__3214EC07F3A94C24");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PaymentIntentId).HasMaxLength(255);
            entity.Property(e => e.Rate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Jobs_Address");

            entity.HasOne(d => d.Client).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Jobs_Client");

            entity.HasOne(d => d.JobManagerUser).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.JobManagerUserId)
                .HasConstraintName("FK_Jobs_JobManager");
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobAppli__3214EC0708EF4051");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.JobApplicationStatus).HasMaxLength(20);
            entity.Property(e => e.ProposalHourlyRate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Viewed).HasDefaultValue(false);
            entity.Property(e => e.WithdrawalStatus).HasMaxLength(20);

            entity.HasOne(d => d.FreelancerUser).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.FreelancerUserId)
                .HasConstraintName("FK_JobApplications_Freelancer");

            entity.HasOne(d => d.Job).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_JobApplications_Job");
        });

        modelBuilder.Entity<JobHire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Hirings");

            entity.HasOne(d => d.Freelancer).WithMany(p => p.JobHireFreelancers)
                .HasForeignKey(d => d.FreelancerId)
                .HasConstraintName("FK_Hirings_Users_FreelancerId");

            entity.HasOne(d => d.HiredManager).WithMany(p => p.JobHireHiredManagers)
                .HasForeignKey(d => d.HiredManagerId)
                .HasConstraintName("FK_Hirings_Users_HiredManagerId");

            entity.HasOne(d => d.Job).WithMany(p => p.JobHires)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_Hirings_Jobs_JobId");
        });

        modelBuilder.Entity<JobQuestionnaire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobQuest__3214EC07631A836B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Job).WithMany(p => p.JobQuestionnaires)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_JobQuestionnaires_Job");
        });

        modelBuilder.Entity<JobQuestionnaireAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobQuest__3214EC0793894954");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.JobApplication).WithMany(p => p.JobQuestionnaireAnswers)
                .HasForeignKey(d => d.JobApplicationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__JobQuesti__JobAp__793DFFAF");

            entity.HasOne(d => d.Question).WithMany(p => p.JobQuestionnaireAnswers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__JobQuesti__Quest__7A3223E8");

            entity.HasOne(d => d.User).WithMany(p => p.JobQuestionnaireAnswers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__JobQuesti__UserI__19FFD4FC");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Notifications).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3213E83FCE5C37C9");

            entity.HasIndex(e => e.StripePaymentIntentId, "UQ__Payments__051F88970FA158B4").IsUnique();

            entity.HasIndex(e => e.StripeTransferId, "UQ__Payments__52C59C526F0B435E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EscrowStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("escrow_status");
            entity.Property(e => e.FailureReason)
                .HasColumnType("text")
                .HasColumnName("failure_reason");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.PaidByUserId).HasColumnName("paid_by_user_id");
            entity.Property(e => e.PaidToUserId).HasColumnName("paid_to_user_id");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("payment_type");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.StripePaymentIntentId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("stripe_payment_intent_id");
            entity.Property(e => e.StripeTransferId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("stripe_transfer_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Job).WithMany(p => p.Payments)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK__Payments__job_id__21A0F6C4");

            entity.HasOne(d => d.PaidByUser).WithMany(p => p.PaymentPaidByUsers)
                .HasForeignKey(d => d.PaidByUserId)
                .HasConstraintName("FK__Payments__paid_b__22951AFD");

            entity.HasOne(d => d.PaidToUser).WithMany(p => p.PaymentPaidToUsers)
                .HasForeignKey(d => d.PaidToUserId)
                .HasConstraintName("FK__Payments__paid_t__23893F36");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Payments__paymen__247D636F");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3213E83F574F6E6B");

            entity.HasIndex(e => e.StripeCardId, "UQ__PaymentM__B61B6229741B50E5").IsUnique();

            entity.HasIndex(e => e.StripePaymentMethodId, "UQ__PaymentM__D66D16C6D459E129").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardBrand)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("card_brand");
            entity.Property(e => e.CardLastFour)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("card_last_four");
            entity.Property(e => e.CardholderName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("cardholder_name");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpMonth).HasColumnName("exp_month");
            entity.Property(e => e.ExpYear).HasColumnName("exp_year");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false)
                .HasColumnName("is_default");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.StripeCardId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("stripe_card_id");
            entity.Property(e => e.StripeCustomerId).HasMaxLength(255);
            entity.Property(e => e.StripePaymentMethodId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("stripe_payment_method_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Client).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__PaymentMe__client__5772F790");

            entity.HasOne(d => d.User).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__PaymentMe__user___20ACD28B");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ratings__3214EC07C383449E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Caregiver).WithMany(p => p.RatingCaregivers)
                .HasForeignKey(d => d.CaregiverId)
                .HasConstraintName("FK_Ratings_Caregivers");

            entity.HasOne(d => d.Facility).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK_Ratings_Clients");

            entity.HasOne(d => d.JobHire).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.JobHireId)
                .HasConstraintName("FK_Ratings_JobHires");

            entity.HasOne(d => d.Job).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_Ratings_Jobs");

            entity.HasOne(d => d.JobManager).WithMany(p => p.RatingJobManagers)
                .HasForeignKey(d => d.JobManagerId)
                .HasConstraintName("FK_Ratings_JobManagers");

            entity.HasOne(d => d.RatingGivenByUser).WithMany(p => p.RatingRatingGivenByUsers)
                .HasForeignKey(d => d.RatingGivenByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ratings_RatingGivenByUser");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK_Ratings_Vendors");
        });

        modelBuilder.Entity<TimeSheet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TimeShee__3214EC071306B7AA");

            entity.ToTable("TimeSheet");

            entity.Property(e => e.ClockIn).HasColumnType("datetime");
            entity.Property(e => e.ClockOut).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeSheetApprovalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeSheetNotes).HasColumnType("text");

            entity.HasOne(d => d.Hiring).WithMany(p => p.TimeSheets)
                .HasForeignKey(d => d.HiringId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TimeSheet__Hirin__257187A8");

            entity.HasOne(d => d.User).WithMany(p => p.TimeSheets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TimeSheet__UserI__2665ABE1");
        });

        modelBuilder.Entity<TimeSheetLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TimeShee__3214EC072B4677C8");

            entity.ToTable("TimeSheetLocation");

            entity.Property(e => e.LocationCapturedDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.TimeSheet).WithMany(p => p.TimeSheetLocations)
                .HasForeignKey(d => d.TimeSheetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TimeSheet__TimeS__2759D01A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07F2E76B03");

            entity.HasIndex(e => e.Auth0UserId, "UQ__Users__1C8F4290EEBCED0F").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534CB342770").IsUnique();

            entity.Property(e => e.Auth0UserId).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Disabled).HasDefaultValue(false);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.StripeAccountId).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.Users)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Users_Address");

            entity.HasOne(d => d.DisabledByUser).WithMany(p => p.InverseDisabledByUser)
                .HasForeignKey(d => d.DisabledByUserId)
                .HasConstraintName("FK_Users_DisabledByUser");
        });

        modelBuilder.Entity<UserDevice>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.UserDevices).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserVendor>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.VendorId });

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.User).WithMany(p => p.UserVendors)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserVendors_UserId");

            entity.HasOne(d => d.Vendor).WithMany(p => p.UserVendors)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserVendors_VendorId");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vendors__3214EC0733FA14CC");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.User).WithMany(p => p.Vendors)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Vendors__UserId__2C1E8537");
        });

        modelBuilder.Entity<VendorsRegistrant>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.User).WithMany(p => p.VendorsRegistrants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VendorsRegistrants_User");

            entity.HasOne(d => d.Vendor).WithMany(p => p.VendorsRegistrants)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VendorsRegistrants_Vendor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
