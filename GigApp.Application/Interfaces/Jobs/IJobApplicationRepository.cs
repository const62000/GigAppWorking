using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Jobs
{
    public interface IJobApplicationRepository
    {
        Task<CreateJobApplicationResult> CreateJobApplicationAsync(CreateJobApplicationRequest request, string AuthoId);
        Task<List<JobApplication>> GetJobApplicationById(long jobId);
        Task<BaseResult> CheckJobApplication(string Auth0Id,long jobId);
        Task<BaseResult> ChangeJobApplicationStatus(long applicationId,string status);
        Task<BaseResult> WithdrawalJobAsync(JobWithdrawalRequest request);
    }
}
