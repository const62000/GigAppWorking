using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Users.Queries
{
    public class GetFreelancers
    {
        public class Query : IQuery
        {
        }
        internal sealed class Handler(IFreelancerRepository _freelancerRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _freelancerRepository.GetFreelancers();
            }
        }
    }
}

