using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Facilities;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Facilities.Queries;

public class DeleteJobManager
{
    public class Query : IQuery
    {
        public long ClientId { get; set; }
        public long UserId { get; set; }
    }
    internal sealed class Handler(IFacilitiesRepository _facilitiesRepository) : IQueryHandler<Query>
    {
        public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _facilitiesRepository.DeleteJobManager(request.ClientId, request.UserId);
            return result;
        }
    }
}