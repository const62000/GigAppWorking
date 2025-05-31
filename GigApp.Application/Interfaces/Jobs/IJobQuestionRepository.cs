using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.JobQuestion;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Responses.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Jobs
{
    public interface IJobQuestionRepository
    {
        Task<CreateJobQuestionResult> CreateJobQuestionAsync(CreateJobQuestionRequest createJobQuestionRequest);
        Task<List<JobQuestionnaire>> GetJobQuestionsById(long jobId);
        Task<List<JobQuestionnaireAnswer>> GetQuestionaireAnswersByJobApplicationIdAsync(long jobApplicationId);
        Task<BaseResult> AddAnswer(JobQuestionnaireAnswer jobQuestionnaireAnswer,string auth0Id);
        Task<BaseResult> DeleteJobQuestions(List<long> ids);
    }
}
