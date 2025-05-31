using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Queries
{
    public class CheckFreelancerApplication
    {
        public class Query : IQuery
        {
            public string Auth0Id { get; set; } = string.Empty;
            public long JobId {  get; set; }
        }
        internal sealed class Handler(IJobApplicationRepository _jobApplicationRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _jobApplicationRepository.CheckJobApplication(request.Auth0Id, request.JobId);
                return result;
            }
        }
    }
}
