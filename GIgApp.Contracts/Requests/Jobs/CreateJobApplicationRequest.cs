using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Jobs
{
    public record CreateJobApplicationRequest(long JobId,long FreelancerUserId, string Proposal, decimal ProposedHourlyRate, List<AnswerInput> Answers);
}
