using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Users.Queries
{
    public class GetBankAccount
    {
        public class Query : IQuery
        {
            public string Auth0Id { get; set; } = string.Empty;
        }
        internal sealed class Handler(IBankAccountRepository _bankAccountRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _bankAccountRepository.GetBankAccount(request.Auth0Id);
                return result;
            }
        }
    }
}
