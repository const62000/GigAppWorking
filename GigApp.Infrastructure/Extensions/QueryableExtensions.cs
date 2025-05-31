using Microsoft.EntityFrameworkCore;
using GigApp.Contracts.Enums;
using GigApp.Domain.Entities;

public static class QueryableExtensions
{
    public static IQueryable<Job> IncludeJobDetails(this IQueryable<Job> query)
    {
        return query
            .Select(j => new Job
            {
                Id = j.Id,
                Title = j.Title,
                StartedDate = j.StartedDate,
                EndedDate = j.EndedDate,
                Description = j.Description,
                Requirements = j.Requirements,
                Date = j.Date,
                Time = j.Time,
                Rate = j.JobType == (int)JobType.Hourly ? j.Rate * (j.HoursPerWeek ?? 0) : j.Rate,
                Status = j.Status,
                LicenseRequirments = j.LicenseRequirments,
                CreatedAt = j.CreatedAt,
                UpdatedAt = j.UpdatedAt,
                JobManagerUserId = j.JobManagerUserId,
                AddressId = j.AddressId,
                ClientId = j.ClientId,
                JobType = j.JobType,
                HoursPerWeek = j.JobType == (int)JobType.Hourly ? j.HoursPerWeek : 0,

                // Include JobManagerUser with specific fields
                JobManagerUser = j.JobManagerUser != null ? new User
                {
                    Id = j.JobManagerUser.Id,
                    Auth0UserId = j.JobManagerUser.Auth0UserId,
                    Email = j.JobManagerUser.Email,
                    FirstName = j.JobManagerUser.FirstName,
                    LastName = j.JobManagerUser.LastName,
                    Picture = j.JobManagerUser.Picture,
                    Disabled = j.JobManagerUser.Disabled,
                    CreatedAt = j.JobManagerUser.CreatedAt,
                    UpdatedAt = j.JobManagerUser.UpdatedAt
                } : null,

                // Include Address with specific fields
                Address = j.Address != null ? new Address
                {
                    Id = j.Address.Id,
                    Label = j.Address.Label,
                    StreetAddress1 = j.Address.StreetAddress1,
                    StreetAddress2 = j.Address.StreetAddress2,
                    City = j.Address.City,
                    StateProvince = j.Address.StateProvince,
                    PostalCode = j.Address.PostalCode,
                    Country = j.Address.Country,
                    Latitude = j.Address.Latitude,
                    Longitude = j.Address.Longitude
                } : null,

                // Include Facility with specific fields
                Client = j.Client != null ? new Client
                {
                    Id = j.Client.Id,
                    Name = j.Client.Name,
                    AdminUserId = j.Client.AdminUserId,
                    CreatedAt = j.Client.CreatedAt,
                    UpdatedAt = j.Client.UpdatedAt
                } : null
            });
    }

    public static IQueryable<Job> IncludeJobApplications(this IQueryable<Job> query)
    {
        return query.Include(j => j.JobApplications);
    }

    public static IQueryable<Job> IncludeJobQuestionnaires(this IQueryable<Job> query)
    {
        return query.Include(j => j.JobQuestionnaires);
    }

    public static IQueryable<User> IncludeUserDetails(this IQueryable<User> query)
    {
        return query
            .Select(u => new User
            {
                Id = u.Id,
                Auth0UserId = u.Auth0UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Picture = u.Picture,
                Disabled = u.Disabled,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,

                // Include Addresses
                Addresses = u.Addresses.Select(a => new Address
                {
                    Id = a.Id,
                    Label = a.Label,
                    StreetAddress1 = a.StreetAddress1,
                    StreetAddress2 = a.StreetAddress2,
                    City = a.City,
                    StateProvince = a.StateProvince,
                    PostalCode = a.PostalCode,
                    Country = a.Country,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude
                }).ToList(),

                // Include FreelancerLicenses
                FreelancerLicenses = u.FreelancerLicenses.Select(l => new FreelancerLicense
                {
                    Id = l.Id,
                    LicenseName = l.LicenseName,
                    LicenseNumber = l.LicenseNumber,
                    IssuedBy = l.IssuedBy,
                    IssuedDate = l.IssuedDate,
                    LicenseStatus = l.LicenseStatus,
                    LicenseFileUrl = l.LicenseFileUrl,
                    RejectionReason = l.RejectionReason
                }).ToList(),

                // Include Jobs
                Jobs = u.Jobs.Select(j => new Job
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Requirements = j.Requirements,
                    Date = j.Date,
                    Time = j.Time,
                    Rate = j.Rate,
                    Status = j.Status
                }).ToList(),

                // Include Companies
                Clients = u.Clients.Select(c => new Client
                {
                    Id = c.Id,
                    Name = c.Name,
                    AdminUserId = c.AdminUserId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).ToList(),
                ClientManagers = u.ClientManagers.Select(cm => new ClientManager
                {
                    Id = cm.Id,
                    ClientId = cm.ClientId,
                    UserId = cm.UserId,
                    Client = cm.Client != null ? new Client
                    {
                        Id = cm.Client.Id,
                        Name = cm.Client.Name,
                        AdminUserId = cm.Client.AdminUserId,
                        CreatedAt = cm.Client.CreatedAt,
                        UpdatedAt = cm.Client.UpdatedAt
                    } : null
                }).ToList(),

                // Include Vendors
                Vendors = u.Vendors.Select(v => new Vendor
                {
                    Id = v.Id,
                    ServicesOffered = v.ServicesOffered,
                    Certifications = v.Certifications,
                    Status = v.Status,
                    CreatedAt = v.CreatedAt
                }).ToList()

            });
    }

    public static IQueryable<JobHire> IncludeJobHireDetails(this IQueryable<JobHire> query)
    {
        return query.Select(j => new JobHire
        {
            Id = j.Id,
            JobId = j.JobId,
            FreelancerId = j.FreelancerId,
            HiredManagerId = j.HiredManagerId,
            Status = j.Status,
            Note = j.Note,
            StartTime = j.StartTime,
            EndTime = j.EndTime,
            Freelancer = j.Freelancer != null ? new User
            {
                Id = j.Freelancer.Id,
                Auth0UserId = j.Freelancer.Auth0UserId,
                Email = j.Freelancer.Email,
                FirstName = j.Freelancer.FirstName,
                LastName = j.Freelancer.LastName,
                Picture = j.Freelancer.Picture,
                Disabled = j.Freelancer.Disabled,
                CreatedAt = j.Freelancer.CreatedAt,
                UpdatedAt = j.Freelancer.UpdatedAt
            } : null,
            HiredManager = new User
            {
                Id = j.HiredManager.Id,
                Auth0UserId = j.HiredManager.Auth0UserId,
                Email = j.HiredManager.Email,
                FirstName = j.HiredManager.FirstName,
                LastName = j.HiredManager.LastName,
                Picture = j.HiredManager.Picture,
                Disabled = j.HiredManager.Disabled,
                CreatedAt = j.HiredManager.CreatedAt,
                UpdatedAt = j.HiredManager.UpdatedAt
            },
            Job = new Job
            {
                Id = j.Job.Id,
                Title = j.Job.Title,
                Description = j.Job.Description,
                Requirements = j.Job.Requirements,
                Date = j.Job.Date,
                Time = j.Job.Time,
                Rate = j.Job.Rate,
                Status = j.Job.Status,
                JobManagerUserId = j.Job.JobManagerUserId,
                AddressId = j.Job.AddressId,
                ClientId = j.Job.ClientId
            },
            TimeSheets = j.TimeSheets.Select(t => new TimeSheet
            {
                Id = t.Id,
                HiringId = t.HiringId,
                UserId = t.UserId,
                ClockIn = t.ClockIn,
                ClockOut = t.ClockOut,
                TimeSheetNotes = t.TimeSheetNotes,
                Status = t.Status,
                TimeSheetApprovalStatus = t.TimeSheetApprovalStatus
            }).ToList()
        });
    }

    public static IQueryable<TimeSheet> IncludeTimeSheetDetails(this IQueryable<TimeSheet> query)
    {
        return query.Select(t => new TimeSheet
        {
            Id = t.Id,
            HiringId = t.HiringId,
            UserId = t.UserId,
            ClockIn = t.ClockIn,
            ClockOut = t.ClockOut,
            TimeSheetNotes = t.TimeSheetNotes,
            Status = t.Status,
            TimeSheetApprovalStatus = t.TimeSheetApprovalStatus,
            Hiring = t.Hiring != null ? new JobHire
            {
                Id = t.Hiring.Id,
                JobId = t.Hiring.JobId,
                FreelancerId = t.Hiring.FreelancerId,
                HiredManagerId = t.Hiring.HiredManagerId,
                Status = t.Hiring.Status,
                Note = t.Hiring.Note,
                StartTime = t.Hiring.StartTime,
                EndTime = t.Hiring.EndTime
            } : null,
            User = t.User != null ? new User
            {
                Id = t.User.Id,
                Auth0UserId = t.User.Auth0UserId,
                Email = t.User.Email,
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,
                Picture = t.User.Picture,
                Disabled = t.User.Disabled
            } : null,
            TimeSheetLocations = t.TimeSheetLocations.Select(tl => new TimeSheetLocation
            {
                Id = tl.Id,
                TimeSheetId = tl.TimeSheetId,
                LocationCapturedDateTime = tl.LocationCapturedDateTime,
                Latitude = tl.Latitude,
                Longitude = tl.Longitude
            }).ToList()
        });
    }

    public static IQueryable<JobApplication> IncludeJobApplicationDetails(this IQueryable<JobApplication> query)
    {
        return query.Select(j => new JobApplication
        {
            Id = j.Id,
            JobId = j.JobId,
            FreelancerUserId = j.FreelancerUserId,
            Proposal = j.Proposal,
            ProposalHourlyRate = j.ProposalHourlyRate,
            Viewed = j.Viewed,
            CreatedAt = j.CreatedAt,
            JobApplicationStatus = j.JobApplicationStatus,
            WithdrawalStatus = j.WithdrawalStatus,
            WithdrawalReason = j.WithdrawalReason,
            WithdrawalDate = j.WithdrawalDate,
            Job = j.Job != null ? new Job
            {
                Id = j.Job.Id,
                Title = j.Job.Title,
                Description = j.Job.Description,
                Requirements = j.Job.Requirements,
                Date = j.Job.Date,
                Time = j.Job.Time,
                Rate = j.Job.Rate,
                Status = j.Job.Status,
                JobManagerUserId = j.Job.JobManagerUserId,
                AddressId = j.Job.AddressId,
                ClientId = j.Job.ClientId
            } : null,
            FreelancerUser = j.FreelancerUser != null ? new User
            {
                Id = j.FreelancerUser.Id,
                Auth0UserId = j.FreelancerUser.Auth0UserId,
                Email = j.FreelancerUser.Email,
                FirstName = j.FreelancerUser.FirstName,
                LastName = j.FreelancerUser.LastName,
                Picture = j.FreelancerUser.Picture,
                Disabled = j.FreelancerUser.Disabled,
                Vendors = j.FreelancerUser.Vendors.Select(v => new Vendor
                {
                    Id = v.Id,
                    ServicesOffered = v.ServicesOffered,
                    Certifications = v.Certifications,
                    Status = v.Status,
                    CreatedAt = v.CreatedAt
                }).ToList()
            } : null,
        });
    }

    public static IQueryable<Rating> IncludeFacilityRatingDetails(this IQueryable<Rating> query)
    {
        return query.Select(r => new Rating
        {
            Id = r.Id,
            RatingGivenByUserId = r.RatingGivenByUserId,
            JobId = r.JobId,
            JobHireId = r.JobHireId,
            JobStarRating = r.JobStarRating,
            FacilityStarRating = r.FacilityStarRating,
            FacilityId = r.FacilityId,
            Facility = r.Facility != null ? new Client
            {
                Id = r.Facility.Id,
                Name = r.Facility.Name,
                AdminUserId = r.Facility.AdminUserId,
                CreatedAt = r.Facility.CreatedAt,
                UpdatedAt = r.Facility.UpdatedAt
            } : null,
            RatingGivenByUser = r.RatingGivenByUser != null ? new User
            {
                Id = r.RatingGivenByUser.Id,
                Auth0UserId = r.RatingGivenByUser.Auth0UserId,
                Email = r.RatingGivenByUser.Email,
                FirstName = r.RatingGivenByUser.FirstName,
                LastName = r.RatingGivenByUser.LastName,
                Picture = r.RatingGivenByUser.Picture,
                Disabled = r.RatingGivenByUser.Disabled
            } : new User(),
            JobHire = r.JobHire != null ? new JobHire
            {
                Id = r.JobHire.Id,
                JobId = r.JobHire.JobId,
                FreelancerId = r.JobHire.FreelancerId,
                HiredManagerId = r.JobHire.HiredManagerId,
                Status = r.JobHire.Status,
                Note = r.JobHire.Note,
                StartTime = r.JobHire.StartTime,
                EndTime = r.JobHire.EndTime
            } : new JobHire(),
            Job = r.Job != null ? new Job
            {
                Id = r.Job.Id,
                Title = r.Job.Title,
                Description = r.Job.Description,
                Requirements = r.Job.Requirements,
                Date = r.Job.Date,
                Time = r.Job.Time,
                Rate = r.Job.Rate,
                Status = r.Job.Status
            } : new Job()
        });
    }

    public static IQueryable<Rating> IncludeCaregiverRatingDetails(this IQueryable<Rating> query)
    {
        return query.Select(r => new Rating
        {
            Id = r.Id,
            RatingGivenByUserId = r.RatingGivenByUserId,
            JobId = r.JobId,
            JobHireId = r.JobHireId,
            JobStarRating = r.JobStarRating,
            CaregiverId = r.CaregiverId,
            CaregiverStarRating = r.CaregiverStarRating,
            RatingGivenByUser = r.RatingGivenByUser != null ? new User
            {
                Id = r.RatingGivenByUser.Id,
                Auth0UserId = r.RatingGivenByUser.Auth0UserId,
                Email = r.RatingGivenByUser.Email,
                FirstName = r.RatingGivenByUser.FirstName,
                LastName = r.RatingGivenByUser.LastName,
                Picture = r.RatingGivenByUser.Picture,
                Disabled = r.RatingGivenByUser.Disabled
            } : new User(),
            JobHire = r.JobHire != null ? new JobHire
            {
                Id = r.JobHire.Id,
                JobId = r.JobHire.JobId,
                FreelancerId = r.JobHire.FreelancerId,
                HiredManagerId = r.JobHire.HiredManagerId,
                Status = r.JobHire.Status,
                Note = r.JobHire.Note,
                StartTime = r.JobHire.StartTime,
                EndTime = r.JobHire.EndTime
            } : new JobHire(),
            Job = r.Job != null ? new Job
            {
                Id = r.Job.Id,
                Title = r.Job.Title,
                Description = r.Job.Description,
                Requirements = r.Job.Requirements,
                Date = r.Job.Date,
                Time = r.Job.Time,
                Rate = r.Job.Rate,
                Status = r.Job.Status
            } : new Job(),
            Caregiver = r.Caregiver != null ? new User
            {
                Id = r.Caregiver.Id,
                Auth0UserId = r.Caregiver.Auth0UserId,
                Email = r.Caregiver.Email,
                FirstName = r.Caregiver.FirstName,
                LastName = r.Caregiver.LastName,
                Picture = r.Caregiver.Picture,
                Disabled = r.Caregiver.Disabled
            } : new User()
        });
    }

    public static IQueryable<Address> IncludeAddressDetails(this IQueryable<Address> query)
    {
        return query.Select(a => new Address
        {
            Id = a.Id,
            Label = a.Label,
            StreetAddress1 = a.StreetAddress1,
            StreetAddress2 = a.StreetAddress2,
            City = a.City,
            StateProvince = a.StateProvince,
            PostalCode = a.PostalCode,
            Country = a.Country,
            Latitude = a.Latitude,
            Longitude = a.Longitude,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            UserId = a.UserId,
        });
    }

    public static IQueryable<PaymentMethod> IncludePaymentMethodDetails(this IQueryable<PaymentMethod> query)
    {
        return query.Select(a => new PaymentMethod
        {
            Id = a.Id,
            CardBrand = a.CardBrand,
            CardLastFour = a.CardLastFour,
            ExpMonth = a.ExpMonth,
            ExpYear = a.ExpYear,
            CardholderName = a.CardholderName,
            StripePaymentMethodId = a.StripePaymentMethodId,
            StripeCardId = a.StripeCardId,
            UserId = a.UserId,
            ClientId = a.ClientId
        });
    }

    public static IQueryable<JobQuestionnaire> IncludeJobQuestionnaireDetails(this IQueryable<JobQuestionnaire> query)
    {
        return query.Select(jq => new JobQuestionnaire
        {
            Id = jq.Id,
            JobId = jq.JobId,
            Question = jq.Question,
            CreatedAt = jq.CreatedAt,
            JobQuestionnaireAnswers = jq.JobQuestionnaireAnswers.Select(jq => new JobQuestionnaireAnswer
            {
                Id = jq.Id,
                Answer = jq.Answer,
                JobApplicationId = jq.JobApplicationId,
                UserId = jq.UserId,
                CreatedAt = jq.CreatedAt,
                QuestionId = jq.QuestionId,
                User = jq.User != null ? new User
                {
                    Id = jq.User.Id,
                    Auth0UserId = jq.User.Auth0UserId,
                    Email = jq.User.Email,
                    FirstName = jq.User.FirstName,
                    LastName = jq.User.LastName,
                    Picture = jq.User.Picture,
                    Disabled = jq.User.Disabled
                } : null
            }).ToList()
        });
    }
    public static IQueryable<JobQuestionnaireAnswer> IncludeJobQuestionnaireAnswerDetails(this IQueryable<JobQuestionnaireAnswer> query)
    {
        return query.Select(jq => new JobQuestionnaireAnswer
        {
            Id = jq.Id,
            Answer = jq.Answer,
            JobApplicationId = jq.JobApplicationId,
            QuestionId = jq.QuestionId,
            UserId = jq.UserId,
            CreatedAt = jq.CreatedAt,
            Question = jq.Question != null ? new JobQuestionnaire
            {
                Id = jq.Question.Id,
                Question = jq.Question.Question,
                CreatedAt = jq.Question.CreatedAt
            } : null,
            User = jq.User != null ? new User
            {
                Id = jq.User.Id,
                Auth0UserId = jq.User.Auth0UserId,
                Email = jq.User.Email,
                FirstName = jq.User.FirstName,
                LastName = jq.User.LastName,
                Picture = jq.User.Picture,
                Disabled = jq.User.Disabled
            } : null
        });
    }

    public static IQueryable<Vendor> IncludeVendorDetails(this IQueryable<Vendor> query)
    {
        return query.Select(v => new Vendor
        {
            Id = v.Id,
            Name = v.Name,
            Status = v.Status,
            Certifications = v.Certifications,
            ServicesOffered = v.ServicesOffered,
            UserId = v.UserId,
            CreatedAt = v.CreatedAt,
            ManagerName = v.User != null ? v.User.FirstName + " " + v.User.LastName : string.Empty
        });
    }
    public static IQueryable<User> IncludeUserVendorDetails(this IQueryable<UserVendor> query)
    {
        return query.Select(uv => new User
        {
            Id = uv.User.Id,
            Email = uv.User.Email,
            FirstName = uv.User.FirstName,
            LastName = uv.User.LastName,
            Picture = uv.User.Picture,
            Disabled = uv.User.Disabled,
            CreatedAt = uv.User.CreatedAt,
            UpdatedAt = uv.User.UpdatedAt,
            VendorName = uv.Vendor.Name
        });
    }
    public static IQueryable<Client> IncludeClientDetails(this IQueryable<Client> query)
    {
        return query.Select(c => new Client
        {

            Id = c.Id,
            Name = c.Name,
            AdminUserId = c.AdminUserId,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            LocationsCount = c.ClientLocations.Count,
            Addresses = c.Addresses.Select(a => new Address
            {
                Id = a.Id,
                Label = a.Label,
                StreetAddress1 = a.StreetAddress1,
                StreetAddress2 = a.StreetAddress2,
                City = a.City,
                StateProvince = a.StateProvince,
                PostalCode = a.PostalCode,
                Country = a.Country,
                Latitude = a.Latitude,
                Longitude = a.Longitude
            }).ToList()
        });
    }
    public static IQueryable<ClientLocation> IncludeClientLocationDetails(this IQueryable<ClientLocation> query)
    {
        return query.Select(cl => new ClientLocation
        {
            Id = cl.Id,
            LocationName = cl.LocationName,
            ClientId = cl.ClientId,
            Address = cl.Address != null ? new Address
            {
                Id = cl.Address.Id,
                Label = cl.Address.Label,
                StreetAddress1 = cl.Address.StreetAddress1,
                StreetAddress2 = cl.Address.StreetAddress2,
                City = cl.Address.City,
                StateProvince = cl.Address.StateProvince,
                PostalCode = cl.Address.PostalCode,
                Country = cl.Address.Country,
                Latitude = cl.Address.Latitude,
                Longitude = cl.Address.Longitude
            } : null
        });
    }


}

