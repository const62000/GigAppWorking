using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.Interfaces.Jobs
{
    public interface IHiringRepository
    {
        Task<BaseResult> HiringByJob(JobHire hiring, string auth0Id);
        Task<BaseResult> GetHiringByJob(long jobId);
        Task<BaseResult> UpdateHiredJobStatus(JobHire hiring);
        Task<BaseResult> GetHiredJobById(int id);
        Task<BaseResult> GetHiringByManager(string auth0Id);
        Task<BaseResult> GetHiringByFreelancer(string auth0Id);
    }
}
