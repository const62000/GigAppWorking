using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Addresses;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Addresses.Queries
{
    public class GetCountries
    {
        public class Query : IQuery
        {

        }
        internal sealed class Handler(IAddressRepository _addressRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _addressRepository.GetAllCountries();
                return result;
            }
        }
    }
}
