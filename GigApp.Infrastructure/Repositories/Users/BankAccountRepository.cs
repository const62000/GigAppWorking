using FirebaseAdmin.Messaging;
using GigApp.Application.Interfaces.Users;
using GigApp.Application.Interfaces.Payments;
using GigApp.Domain.Entities;
using GigApp.Contracts.Enums;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using GIgApp.Contracts.Requests.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Infrastructure.Repositories.Users
{
    public class BankAccountRepository(ApplicationDbContext _applicationDbContext, IUserRepository _userRepository, IStripePaymentRepository _stripePaymentRepository) : IBankAccountRepository
    {
        public async Task<BaseResult> AddBankAccount(BankAccount bankAccount, string auth0Id, string bankToken)
        {
            var user = await GetUser(auth0Id);
            if (string.IsNullOrEmpty(bankToken))
            {
                var bankAccountToken = await _stripePaymentRepository.CreateBankAccountToken(bankAccount.BankSwiftCode, bankAccount.BankAccountNumber, user.FirstName + " " + user.LastName);
                if (!bankAccountToken.Status)
                    return new BaseResult(new { }, false, bankAccountToken.Message);
                bankToken = bankAccountToken.Data as string;
            }
            var customerResult = await _stripePaymentRepository.CreateCustomerWithBankAccount(bankToken, user);
            if (!customerResult.Status)
                return customerResult;
            var stripeBankAccount = customerResult.Data as StripeBankAccount;
            bankAccount.StripeCustomerId = stripeBankAccount.CustomerId;
            bankAccount.StripeBankAccountId = stripeBankAccount.BankAccountId;
            bankAccount.UserId = user.Id;
            await _applicationDbContext.BankAccounts.AddAsync(bankAccount);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { bankAccount.Id }, true, Messages.BankAccount_Created);
        }
        public async Task<BaseResult> VerifyBankAccount(int bankAccountId, List<long?> amounts)
        {
            var bankAccount = await _applicationDbContext.BankAccounts.Where(x => x.Id == bankAccountId).FirstOrDefaultAsync();
            if (bankAccount == null)
                return new BaseResult(new { }, false, Messages.BankAccount_Not_Found);
            var result = await _stripePaymentRepository.VerifyBankAccount(bankAccount.StripeBankAccountId, bankAccount.StripeCustomerId, amounts);
            if (!result.Status)
                return result;
            bankAccount.Status = BankAccountStatus.Verified.ToString();
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, Messages.BankAccount_Verified);
        }
        public async Task<BaseResult> DeleteBankAccount(int acoountId)
        {
            var result = await _applicationDbContext.BankAccounts.Where(x => x.Id == acoountId).ExecuteDeleteAsync();
            if (result > 0)
                return new BaseResult(new { }, true, Messages.BankAccount_Deleted);
            return new BaseResult(new { }, true, Messages.BankAccount_Deleted_Fail);
        }

        public async Task<BaseResult> GetBankAccount(string auth0Id)
        {
            var userId = await _userRepository.GetUserId(auth0Id);
            var bankAccount = await _applicationDbContext.BankAccounts.Where(x => x.UserId == userId).ToListAsync();
            return new BaseResult(bankAccount, true, string.Empty);
        }

        public async Task<BaseResult> UpdateBankAccount(BankAccount bankAccount)
        {
            var result = await _applicationDbContext.BankAccounts.Where(x => x.Id == bankAccount.Id).ExecuteUpdateAsync(s =>
            s.SetProperty(x => x.BankAccountNumber, bankAccount.BankAccountNumber)
            .SetProperty(x => x.BankSwiftCode, bankAccount.BankSwiftCode)
            .SetProperty(x => x.BankAccountName, bankAccount.BankAccountName)
            .SetProperty(x => x.BankName, bankAccount.BankName)
            .SetProperty(x => x.BankCountry, bankAccount.BankCountry)
            .SetProperty(x => x.Status, bankAccount.Status)
            .SetProperty(x => x.BankAccountType, bankAccount.BankAccountType)
                );
            if (result > 0)
                return new BaseResult(new { bankAccount.Id }, true, Messages.BankAccount_Updated);
            else
                return new BaseResult(new { }, false, Messages.BankAccount_Updated_Fail);
        }

        private async Task<User> GetUser(string auth0Id)
        {
            var user = await _applicationDbContext.Users.Include(x => x.Addresses).FirstOrDefaultAsync(x => x.Auth0UserId == auth0Id);
            if (user == null)
                return new User();
            return user;
        }
    }
}
