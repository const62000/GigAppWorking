using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Users
{
    public interface IBankAccountRepository
    {
        Task<BaseResult> AddBankAccount(BankAccount bankAccount,string auth0Id,string bankToken);
        Task<BaseResult> GetBankAccount(string auth0Id);
        Task<BaseResult> UpdateBankAccount(BankAccount bankAccount);
        Task<BaseResult> DeleteBankAccount(int acoountId);
        Task<BaseResult> VerifyBankAccount(int bankAccountId,List<long?> amounts);
    }
}
