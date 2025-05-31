using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Users;
using GigApp.Application.Options.Database;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GigApp.Infrastructure.Repositories.Jobs
{
    public class JobApplicationRepository(ApplicationDbContext _context, IUserRepository _userRepository) : IJobApplicationRepository
    {
        // public async Task<BaseResult> CheckJobApplication(string Auth0Id, long jobId)
        // {
        //     var user = await _context.Users.Where(x => x.Auth0UserId == Auth0Id).FirstOrDefaultAsync()?? new User();
        //     var freelancer = await _context.Freelancers.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
        //     if (freelancer != null)
        //     {
        //         if (await _context.JobApplications.AnyAsync(x => x.FreelancerUserId == freelancer.UserId && x.JobId == jobId))
        //             return new BaseResult(new { }, false, string.Empty);
        //         return new BaseResult(new { }, true, string.Empty);
        //     }
        //     return new BaseResult(new { }, false, Messages.Not_Freelancer);
        // }

        public async Task<BaseResult> CheckJobApplication(string auth0Id, long jobId)
        {
            var user = await _context.Users.Where(x => x.Auth0UserId == auth0Id).FirstOrDefaultAsync()?? new User();
            var status = await _context.JobApplications.AnyAsync(x => x.JobId == jobId && x.FreelancerUserId == user.Id);
            if (status)
                return new BaseResult(new { }, false, Messages.JobApplication_AlreadyApplied);
            var job = await _context.Jobs.Where(x => x.Id == jobId).FirstOrDefaultAsync();
            if(job.JobManagerUserId == user.Id)
                return new BaseResult(new { }, false, Messages.You_Are_The_Job_Manager);
            if(job.StartedDate != null && job.EndedDate != null)
            {
                status = await _context.JobApplications.AnyAsync(x => x.Job.StartedDate >= job.StartedDate && x.Job.EndedDate <= job.EndedDate);
                if (status)
                    return new BaseResult(new { }, false, Messages.You_Are_Occupied_With_Another_Job);
            }
            return new BaseResult(new { }, true, Messages.You_Are_Eligible_To_Apply);
        }

        public async Task<CreateJobApplicationResult> CreateJobApplicationAsync(CreateJobApplicationRequest request, string Auth0Id)
        {
            try
            {
                long userId = await _userRepository.GetUserId(Auth0Id);

                if (!await _context.Freelancers.AnyAsync(x => x.UserId == Convert.ToInt32(userId)))
                {
                    return new CreateJobApplicationResult(0, "You don't have a freelancer account");
                }


                var jobApplication = new JobApplication
                {
                    //FreelancerUserId = request.FreelancerUserId,
                    FreelancerUserId = userId,
                    Proposal = request.Proposal,
                    ProposalHourlyRate = request.ProposedHourlyRate,
                    CreatedAt = DateTime.UtcNow,
                };

                var job = await _context.Jobs.FindAsync(request.JobId);
                if (job == null)
                {
                    throw new ArgumentException("Job not found.");
                }

                // Assign the job to the application
                jobApplication.JobId = request.JobId;
                jobApplication.JobApplicationStatus = job.Status;

                // Add the new job application to the context
                _context.JobApplications.Add(jobApplication);
                await _context.SaveChangesAsync();
                // Process answers
                if (request.Answers != null)
                {
                    foreach (var answer in request.Answers)
                    {
                        var jobQuestionnaireAnswer = new JobQuestionnaireAnswer
                        {
                            JobApplicationId = jobApplication.Id, // Set the foreign key
                            QuestionId = answer.JobQuestionId,
                            UserId = userId, // Assuming this is the user answering the question
                            Answer = answer.Answer,
                            CreatedAt = DateTime.UtcNow // Or use a default value based on your needs
                        };

                        _context.JobQuestionnaireAnswers.Add(jobQuestionnaireAnswer);
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return new CreateJobApplicationResult(jobApplication.Id, "Application submitted successfully");
            }
            catch (DbUpdateException ex)
            {
                return new CreateJobApplicationResult(0, "An error occurred while creating the job: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Catch any other exceptions
                return new CreateJobApplicationResult(0, "An unexpected error occurred: " + ex.Message);
            }

        }
        public async Task<List<JobApplication>> GetJobApplicationById(long jobId)
        {
            try
            {
                var jobApplicationDetails = await _context.JobApplications.Where(ja => ja.JobId == jobId).IncludeJobApplicationDetails()
                        .ToListAsync();

                // Check if job application exists
                if (jobApplicationDetails == null)
                {
                    throw new KeyNotFoundException($"Job application with ID {jobId} not found.");
                }

                return jobApplicationDetails;
            }
            catch (KeyNotFoundException knfEx)
            {
                // Handle not found exceptions specifically
                throw new Exception($"Error: {knfEx.Message}", knfEx);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-related exceptions
                throw new Exception("A database error occurred while fetching job application details.", dbEx);
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions
                throw new Exception("An error occurred while fetching job application details: " + ex.Message, ex);
            }
        }

        public async Task<BaseResult> ChangeJobApplicationStatus(long jobId,string status)
        {
            var result = await _context.JobApplications.Where(x => x.JobId == jobId).ExecuteUpdateAsync(s =>
            s.SetProperty(x => x.JobApplicationStatus, status)
                );
            if (result > 0)
                return  new BaseResult(new { }, true, string.Empty); 
            else 
                return new BaseResult(new { }, false, Messages.JobApplication_NotFount);
        }

        public async Task<BaseResult> WithdrawalJobAsync(JobWithdrawalRequest request)
        {
            try
            {
                var result = await _context.JobApplications
                .Where(x => x.Id == request.JobApplicationId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.WithdrawalReason, request.WithdrawalReason)
                    .SetProperty(x => x.WithdrawalStatus, "true")
                    .SetProperty(x => x.WithdrawalDate, DateTime.UtcNow)
                );
                if (result > 0)
                    return new BaseResult(new { }, true, string.Empty);
                else
                    return new BaseResult(new { }, false, Messages.JobApplication_NotFount);
            }
            catch (Exception ex)
            {
                return new BaseResult(new { }, false, $"An error occurred: {ex.Message}");
            }
        }
    }
}
