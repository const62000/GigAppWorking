using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Queries;

public class CompleteJob
{
    public class Query : IQuery
    {
        public long JobId { get; set; }
    }
    internal sealed class Handler(IJobRepository _jobRepository) : IQueryHandler<Query>
    {
        public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _jobRepository.CompleteJobAsync(request.JobId);
        }
    }
}