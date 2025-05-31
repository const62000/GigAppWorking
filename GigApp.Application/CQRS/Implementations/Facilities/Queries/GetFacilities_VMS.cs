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
    public class GetFacilities_VMS
    {
        public class Query : IQuery
        {
            
        }
        internal sealed class Handler(IFacilitiesRepository _facilitiesRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _facilitiesRepository.GetFacilities();
                return result;
            }
        }
    }
}
