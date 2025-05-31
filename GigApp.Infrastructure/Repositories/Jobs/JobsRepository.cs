using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Users;
using GigApp.Application.Options.Auth0;
using GigApp.Application.Options.Database;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GigApp.Contracts.Enums;
using GIgApp.Contracts.Shared;
using System.Text.Json;
using GigApp.Application.Interfaces.Payments;
using Stripe;

namespace GigApp.Infrastructure.Repositories.Jobs;

public class JobsRepository(JsonSerializerOptions options, ApplicationDbContext _context, IUserRepository _userRepository, IStripePaymentRepository _stripePaymentRepository) : IJobRepository
{
    public async Task<BaseResult> CompleteJobAsync(long jobId)
    {
        var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);

        if (job == null)
            return new BaseResult(new { }, false, Messages.Job_NotFound);

        var result = await _stripePaymentRepository.ReleaseEscrowPayment(job.PaymentIntentId);
        if (!result.Status)
            return result;


        job.Status = JobStatus.Completed.ToString();
        var paymentIntent = result.Data as PaymentIntent;
        var payment = new Payment
        {
            JobId = job.Id,
            PaymentMethodId = job.PaymentMethodId,
            Amount = paymentIntent.Amount,
            Status = PaymentStatus.Completed.ToString(),
            PaymentType = PaymentType.Escrow.ToString(),
            StripePaymentIntentId = job.PaymentIntentId,
            StripeTransferId = string.Empty,
            Description = "Job completed successfully",
            PaidByUserId = job.JobManagerUserId
        };
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return new BaseResult(new { }, true, Messages.Job_Completed);
    }
    public async Task<CreateJobResult> CreateJobAsync(CreateJobRequest createJobRequest, string authoId)
    {
        try
        {
            long userId = await _userRepository.GetUserId(authoId);
            var user = await _context.Users.FindAsync(userId);
            var job = new Job
            {
                Title = createJobRequest.Title,
                LicenseRequirments = createJobRequest.LicenseRequirments,
                Description = createJobRequest.Description,
                Requirements = createJobRequest.Requirements,
                Date = createJobRequest.Date,
                Time = createJobRequest.Time,
                Rate = createJobRequest.Rate,
                Status = createJobRequest.Status,
                JobManagerUserId = userId,
                AddressId = createJobRequest.AddressId,
                JobType = (int)createJobRequest.JobType,
                HoursPerWeek = createJobRequest.HoursPerWeek,
                PaymentMethodId = createJobRequest.PaymentMethodId,
                StartedDate = createJobRequest.StartedDate,
                EndedDate = createJobRequest.EndedDate
            };
            if (createJobRequest.FacilityId > 0)
                job.ClientId = createJobRequest.FacilityId;

            var amount = job.JobType == (int)JobType.Hourly ? createJobRequest.Rate * createJobRequest.HoursPerWeek : createJobRequest.Rate;
            var paymentMethod = await _context.PaymentMethods.FirstOrDefaultAsync(x => x.Id == createJobRequest.PaymentMethodId);
            var result = await _stripePaymentRepository.CreateEscrowPaymentIntent(user.StripeCustomerId, Convert.ToInt64(amount), paymentMethod.StripePaymentMethodId);
            if (!result.Status)
                return new CreateJobResult(0, "Failed to create payment intent: " + result.Message);
            var paymentIntent = result.Data as PaymentIntent;
            job.PaymentIntentId = paymentIntent.Id;
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();

            return new CreateJobResult(job.Id, "Job Created successfully!");
        }
        catch (DbUpdateException ex)
        {
            return new CreateJobResult(0, "An error occurred while creating the job: " + ex.Message);
        }
        catch (Exception ex)
        {
            // Catch any other exceptions
            return new CreateJobResult(0, "An unexpected error occurred: " + ex.Message);
        }
    }

    public async Task<Job> GetJobDetailsAsync(long jobId)
    {
        try
        {
            //var jobDetails = await _context.Jobs.FindAsync(jobId);
            var jobDetails = await _context.Jobs
                .IncludeJobDetails()
                .FirstOrDefaultAsync(j => j.Id == jobId);

            // Check if job exists
            if (jobDetails == null)
            {
                throw new KeyNotFoundException($"Job with ID {jobId} not found.");
            }

            return jobDetails;
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            throw new Exception("An error occurred while fetching job details: " + ex.Message);
        }
    }

    public async Task<List<Job>> ListJobsAsync(JobSearchRequest command)
    {
        try
        {
            var query = _context.Jobs
                .IncludeJobDetails()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(command.SearchQuery))
            {
                query = query.Where(j => j.Title.Contains(command.SearchQuery) ||
                                       j.Description.Contains(command.SearchQuery));
            }

            // Fetch data from the database
            var jobs = await query.ToListAsync();

            // Apply the distance filter on the client side
            if (command.Latitude != 0 && command.Longitude != 0 && command.Miles > 0)
            {
                double earthRadius = 3958.8; // Radius of the Earth in miles

                jobs = jobs.Where(j =>
                    j.Address != null &&
                    (decimal)(earthRadius * Math.Acos(
                        Math.Cos(DegreesToRadians((double)command.Latitude)) * Math.Cos(DegreesToRadians((double)j.Address.Latitude.Value)) *
                        Math.Cos(DegreesToRadians((double)j.Address.Longitude.Value) - DegreesToRadians((double)command.Longitude)) +
                        Math.Sin(DegreesToRadians((double)command.Latitude)) * Math.Sin(DegreesToRadians((double)j.Address.Latitude.Value))
                    )) <= command.Miles).ToList();
            }

            // Apply filters
            foreach (var filter in command.Filters)
            {
                switch (filter.Key)
                {
                    case "byCountry":
                        jobs = jobs.Where(x => x.Address != null && x.Address!.Country == filter.Value).ToList();
                        break;
                    case "byCity":
                        jobs = jobs.Where(x => x.Address != null && x.Address!.City == filter.Value).ToList();
                        break;
                    case "byLicenseRequirements":
                        jobs = jobs.Where(x => x.LicenseRequirments != null && x.LicenseRequirments!.Contains(filter.Value)).ToList();
                        break;
                    case "status":
                        jobs = jobs.Where(j => j.Status != null && j.Status.ToLower() == filter.Value.ToLower()).ToList();
                        break;
                }
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(command.SortBy))
            {
                var normalizedSortBy = command.SortBy.ToLower();
                switch (normalizedSortBy)
                {
                    case "title":
                        jobs = command.Ascending ? jobs.OrderBy(j => j.Title).ToList() : jobs.OrderByDescending(j => j.Title).ToList();
                        break;
                    case "date":
                        jobs = command.Ascending ? jobs.OrderBy(j => j.Date).ToList() : jobs.OrderByDescending(j => j.Date).ToList();
                        break;
                    default:
                        break;
                }
            }

            if (command.Ascending)
                jobs = jobs.OrderBy(j => j.CreatedAt).ToList();
            else
                jobs = jobs.OrderByDescending(j => j.CreatedAt).ToList();

            // Apply pagination
            jobs = jobs.Skip((command.Page - 1) * command.PerPage)
                       .Take(command.PerPage)
                       .ToList();

            return jobs;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while fetching jobs.", ex);
        }
    }
    public async Task<BaseResult> UpdateJobAsync(UpdateJobRequest updateJobRequest)
    {
        try
        {
            // Find the existing job by Id
            var job = await _context.Jobs.FindAsync(updateJobRequest.JobId);

            // Check if the job exists
            if (job == null)
            {
                throw new KeyNotFoundException($"Job with ID {updateJobRequest.JobId} not found.");
            }

            // Update job properties
            job.Title = updateJobRequest.Title;
            job.Description = updateJobRequest.Description;
            job.Requirements = updateJobRequest.Requirements;
            job.Date = updateJobRequest.Date;
            job.Time = updateJobRequest.Time;
            job.Rate = updateJobRequest.Rate;
            job.Status = updateJobRequest.Status;
            job.JobType = (int)updateJobRequest.JobType;
            job.HoursPerWeek = updateJobRequest.HoursPerWeek;

            // Save changes
            await _context.SaveChangesAsync();

            return new BaseResult(new { JobId = updateJobRequest.JobId }, true, "Job data updated successfully!");
        }
        catch (DbUpdateException ex)
        {
            return new BaseResult(0, false, "An error occurred while updating the job: " + ex.Message);
        }
        catch (Exception ex)
        {
            // Catch any other exceptions
            return new BaseResult(0, false, "An unexpected error occurred: " + ex.Message);
        }
    }
    public async Task<BaseResult> DeleteJobAsync(long jobId)
    {
        try
        {
            // Find the job in the database
            var job = await _context.Jobs.Where(x => x.Id == jobId).ExecuteDeleteAsync();

            // Check if the job was found
            if (job > 0)
            {
                return new BaseResult(jobId, true, "Job deleted successfully!");
            }
            else
            {
                return new BaseResult(new { }, false, $"Job with ID {jobId} not found.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the job.", ex);
        }
    }

    public async Task<IEnumerable<Job>> GetJobsByStatusAsync(string status)
    {
        try
        {
            var jobs = await _context.Jobs
           .Where(job => job.Status.ToLower() == status.ToLower())
           .ToListAsync();

            if (jobs == null || !jobs.Any())
            {
                return Enumerable.Empty<Job>();
            }
            return jobs;
        }
        catch (Exception ex)
        {
            // Return an empty list or rethrow the exception based on  needs
            throw new ApplicationException("An error occurred while fetching jobs.", ex);
        }

    }

    public async Task<BaseResult> DeleteJobsAsync(List<long> ids)
    {

        try
        {
            // Perform the delete operation using ExecuteDeleteAsync
            var deleteCount = await _context.Jobs
                .Where(job => ids.Contains(job.Id))
                .ExecuteDeleteAsync();

            if (deleteCount > 0)
            {
                return new BaseResult(deleteCount, true, $"{deleteCount} jobs deleted successfully.");
            }
            else
            {
                return new BaseResult(new { }, false, "No jobs found to delete.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the job.", ex);
        }
    }

    public async Task<BaseResult> AssignJobManagerAsync(AssignManagerRequest request)
    {
        try
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == request.JobId);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (job == null || user == null)
            {
                return new BaseResult(new { }, false, "No jobs or user found.");
            }
            job.JobManagerUserId = request.UserId;
            await _context.SaveChangesAsync();

            return new BaseResult(job.JobManagerUserId, true, "Job manager assigned successfully");
        }
        catch (Exception ex)
        {
            return new BaseResult(new { }, false, $"An unexpected error occurred: {ex.Message}");
        }

    }

    private double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}