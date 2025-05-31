using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Notifications;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;


namespace GigApp.Infrastructure.Repositories.Jobs
{
    internal class HiringRepository(ApplicationDbContext _applicationDbContext, IJobApplicationRepository _jobApplicationRepository, INotificationRepository _notificationRepository) : IHiringRepository
    {
        public async Task<BaseResult> GetHiringByJob(long jobId)
        {
            var hiringHistories = await _applicationDbContext.JobHires.Where(x => x.JobId == jobId).IncludeJobHireDetails().ToListAsync();
            return new BaseResult(hiringHistories, true, string.Empty);
        }

        public async Task<BaseResult> GetHiringByFreelancer(string auth0Id)
        {
            var user = await _applicationDbContext.Users.Where(x => x.Auth0UserId == auth0Id).FirstOrDefaultAsync();
            if (user == null)
                return new BaseResult(new { }, false, Messages.User_Not_Found);
            var hiringHistories = await _applicationDbContext.JobHires.Where(x => x.FreelancerId == user.Id).IncludeJobHireDetails().ToListAsync();
            return new BaseResult(hiringHistories, true, string.Empty);
        }

        public async Task<BaseResult> GetHiringByManager(string auth0Id)
        {
            var user = await _applicationDbContext.Users.Where(x => x.Auth0UserId == auth0Id).FirstOrDefaultAsync() ?? new User();
            var hiringHistories = await _applicationDbContext.JobHires.Where(x => x.HiredManagerId == user.Id).IncludeJobHireDetails().ToListAsync();
            return new BaseResult(hiringHistories, true, string.Empty);
        }

        public async Task<BaseResult> UpdateHiredJobStatus(JobHire hiring)
        {
            var result = await _applicationDbContext.JobHires.Where(x => hiring.Id == x.Id).ExecuteUpdateAsync(s =>
            s.SetProperty(x => x.Status, hiring.Status)
            .SetProperty(x => x.Note, hiring.Note));
            if (result > 0)
                return new BaseResult(new { hiring.Id }, true, Messages.Hiring_Status);
            else
                return new BaseResult(new { }, false, Messages.Hiring_Fail_Status);
        }

        public async Task<BaseResult> GetHiredJobById(int id)
        {
            var hiring = await _applicationDbContext.JobHires.IncludeJobHireDetails().FirstOrDefaultAsync(x => x.Id == id) ?? new JobHire();
            return new BaseResult(hiring, true, string.Empty);
        }

        public async Task<BaseResult> HiringByJob(JobHire hiring, string auth0Id)
        {
            var user = await _applicationDbContext.Users.Where(x => x.Auth0UserId == auth0Id).FirstOrDefaultAsync() ?? new User();
            var result = await _jobApplicationRepository.ChangeJobApplicationStatus(hiring.JobId, "Hired");
            if (!result.Status)
                return new BaseResult(new { }, false, Messages.JobApplication_NotFount);
            if (await _applicationDbContext.JobHires.AnyAsync(x => x.FreelancerId == hiring.FreelancerId && x.Job.Id == hiring.JobId))
                return new BaseResult(new { }, false, Messages.Freelancer_Already_Hired);
            hiring.HiredManagerId = user.Id;
            await _applicationDbContext.JobHires.AddAsync(hiring);
            await _applicationDbContext.SaveChangesAsync();
            await _notificationRepository.JobHiredNotification(hiring.JobId, hiring.FreelancerId!.Value, hiring.HiredManagerId);
            return new BaseResult(new { hiring.Id }, true, Messages.Hiring_Created);
        }
    }
}
