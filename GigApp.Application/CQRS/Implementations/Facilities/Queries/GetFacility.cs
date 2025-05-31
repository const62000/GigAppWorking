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
    public class GetFacility
    {
        public class Query : IQuery
        {
            public long Id { get; set; }
        }
        internal sealed class Handler(IFacilitiesRepository _facilitiesRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _facilitiesRepository.GetFacility(request.Id);
                return result;
            }
        }
    }
}
