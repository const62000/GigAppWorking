using System;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.Interfaces.Jobs;

public interface IJobRepository
{
    Task<CreateJobResult> CreateJobAsync(CreateJobRequest job, string authoId);

    Task<Job> GetJobDetailsAsync(long jobId);

    Task<List<Job>> ListJobsAsync(JobSearchRequest jobSearchRequest);

    Task<BaseResult> UpdateJobAsync(UpdateJobRequest updateJobRequest);

    Task<BaseResult> DeleteJobAsync(long jobId);
    Task<IEnumerable<Job>> GetJobsByStatusAsync(string status);
    Task<BaseResult> DeleteJobsAsync(List<long> ids);
    Task<BaseResult> AssignJobManagerAsync(AssignManagerRequest request);
    Task<BaseResult> CompleteJobAsync(long jobId);

}
