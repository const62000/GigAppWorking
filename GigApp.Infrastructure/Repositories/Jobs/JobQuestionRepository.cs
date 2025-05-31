using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Users;
using GigApp.Application.Options.Database;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Requests.JobQuestion;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Responses.Jobs;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancerLicense = GIgApp.Contracts.Responses.Jobs.FreelancerLicense;

namespace GigApp.Infrastructure.Repositories.Jobs
{
    public class JobQuestionRepository(ApplicationDbContext _context, IUserRepository _userRepository) : IJobQuestionRepository
    {
        public async Task<BaseResult> AddAnswer(JobQuestionnaireAnswer jobQuestionnaireAnswer, string auth0Id)
        {
            var userId = await _userRepository.GetUserId(auth0Id);
            jobQuestionnaireAnswer.UserId = userId;
            await _context.JobQuestionnaireAnswers.AddAsync(jobQuestionnaireAnswer);
            await _context.SaveChangesAsync();
            return new BaseResult(new { }, true, Messages.Add_Answer);
        }

        public async Task<CreateJobQuestionResult> CreateJobQuestionAsync(CreateJobQuestionRequest createJobQuestionRequest)
        {
            try
            {
                var jobQuestion = new JobQuestionnaire
                {
                    Question = createJobQuestionRequest.Title,
                    JobId = createJobQuestionRequest.JobId,
                    CreatedAt = DateTime.UtcNow,
                };
                await _context.JobQuestionnaires.AddAsync(jobQuestion);
                await _context.SaveChangesAsync();

                return new CreateJobQuestionResult(jobQuestion.Id, true);
            }
            catch (Exception)
            {
                return new CreateJobQuestionResult(0, false);
            }
        }

        public async Task<BaseResult> DeleteJobQuestions(List<long> ids)
        {
            await _context.JobQuestionnaireAnswers.Where(x => ids.Contains((long)x.QuestionId!)).ExecuteDeleteAsync();
            var result = await _context.JobQuestionnaires.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
            if (result > 0)
                return new BaseResult(new { }, true, Messages.JobQuestions_Deleted);
            return new BaseResult(new { }, true, Messages.JobQuestions_Deleted_Fail);
        }

        public async Task<List<JobQuestionnaire>> GetJobQuestionsById(long jobId)
        {
            try
            {
                var jobQuestions = await _context.JobQuestionnaires.Where(jq => jq.JobId == jobId)
                        .IncludeJobQuestionnaireDetails()
                        .ToListAsync();

                // Check if job application exists
                if (jobQuestions == null)
                {
                    throw new KeyNotFoundException($"Job question with ID {jobId} not found.");
                }

                return jobQuestions;
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions
                throw new Exception("An error occurred while fetching job questions: " + ex.Message, ex);
            }
        }

        public async Task<List<JobQuestionnaireAnswer>> GetQuestionaireAnswersByJobApplicationIdAsync(long jobApplicationId)
        {
            try
            {
                var jobApplication = await _context.JobApplications
                    .Where(ja => ja.Id == jobApplicationId)
                    .FirstOrDefaultAsync();

                if (jobApplication != null)
                {
                    // Set the Viewed status to true
                    jobApplication.Viewed = true;

                    // Save changes to update the Viewed flag in the database
                    await _context.SaveChangesAsync();
                }
                var answers = await _context.JobQuestionnaireAnswers
                    .Where(a => a.JobApplicationId == jobApplicationId)
                    .IncludeJobQuestionnaireAnswerDetails()
                    .ToListAsync();


                //        .ThenInclude(u => u.FreelancerLicenses) // Include FreelancerLicenses for the User
                //    .Select(a => new JobQuestionnaireAnswerResult
                //    {
                //        Id = a.Id,
                //        JobApplicationId = a.JobApplicationId.Value,
                //        QuestionId = a.QuestionId.Value,
                //        UserId = a.UserId.Value,
                //        Answer = a.Answer,
                //        CreatedAt = a.CreatedAt.Value,
                //        QuestionData = new QuestionData
                //        {
                //            Id = a.Question.Id,
                //            JobId = a.Question.JobId.Value,
                //            Title = a.Question.Question
                //        },
                //        UserData = new UserData
                //        {
                //            Id = a.User.Id,
                //            FirstName = a.User.FirstName ?? string.Empty,
                //            LastName = a.User.LastName ?? string.Empty
                //        },
                //        FreelancerLicenseData = a.User.FreelancerLicenses
                //            .Where(fl => fl.FreelancerUserId == a.UserId) // Filter FreelancerLicenses based on UserId
                //            .Select(fl => new FreelancerLicense
                //            {
                //                Id = fl.Id,
                //                FreelancerUserId = fl.FreelancerUserId,
                //                LicenseName = fl.LicenseName,
                //                LicenseNumber = fl.LicenseNumber,
                //                IssuedBy = fl.IssuedBy,
                //                IssuedDate = fl.IssuedDate,
                //                LicenseStatus = fl.LicenseStatus,
                //                RejectionReason = fl.RejectionReason,
                //                LicenseFileUrl = fl.LicenseFileUrl
                //            }).ToList() // Return all matching FreelancerLicenses as a list
                //    })
                //    .ToListAsync();

                return answers;
            }
            catch (DbUpdateException)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.");
            }
            catch (InvalidOperationException)
            {
                throw new Exception("An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw new Exception("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
