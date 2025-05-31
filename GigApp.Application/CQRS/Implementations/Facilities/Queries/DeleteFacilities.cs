using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Facilities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Facilities.Queries
{
    public class DeleteFacilities
    {
        public class Query : IQuery
        {
            public List<long> ClientIds { get; set; } = new List<long>();
        }
        internal sealed class Handler(IFacilitiesRepository _facilitiesRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _facilitiesRepository.DeleteFacilities(request.ClientIds);
                return result;
            }

        }
    }
}
