using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Users.Queries
{
    public class VerifyBankAccount
    {
        public class Query : IQuery
        {
            public int BankAccountId { get; set; }
            public List<long?> Amounts { get; set; }
        }
        internal sealed class Handler(IBankAccountRepository _bankAccountRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _bankAccountRepository.VerifyBankAccount(request.BankAccountId, request.Amounts);
                return result;
            }
        }
    }
}