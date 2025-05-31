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
    public class DeleteBankAccount
    {
        public class Query : IQuery
        {
            public int Id { get; set; }
        }
        internal sealed class Handler(IBankAccountRepository _bankAccountRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _bankAccountRepository.DeleteBankAccount(request.Id);
                return result;
            }
        }
    }
}
