using GigApp.Application.Interfaces.Payments;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Infrastructure.Repositories.Payments
{
    internal class PaymentRepository(ApplicationDbContext _applicationDbContext, IStripePaymentRepository _stripePaymentRepository) : IPaymentRepository
    {
        public async Task<BaseResult> AddPaymentMethod(PaymentMethod paymentMethod, string auth0Id)
        {
            var user = await GetUser(auth0Id);
            if (user.Id < 1)
                return new BaseResult(new { }, false, Messages.User_Not_Found);

            paymentMethod.UserId = user.Id;
            if (string.IsNullOrEmpty(user.StripeCustomerId))
            {
                var customerResult = await _stripePaymentRepository.CreateCustomer(paymentMethod.StripePaymentMethodId, user.Email);
                if (!customerResult.Status)
                    return customerResult;
                user.StripeCustomerId = customerResult.Data as string;
            }
            else
            {
                var result = await _stripePaymentRepository.AttachPaymentMethodToCustomer(user.StripeCustomerId, paymentMethod.StripePaymentMethodId);
                if (!result.Status)
                    return result;
            }
            await _applicationDbContext.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x.SetProperty(y => y.StripeCustomerId, user.StripeCustomerId));
            await _applicationDbContext.PaymentMethods.AddAsync(paymentMethod);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, Messages.PaymentMethod_Created);
        }

        public async Task<BaseResult> CheckPaymentMethod(string auth0Id)
        {
            var userId = await GetUserId(auth0Id);
            return new BaseResult(new { }, await _applicationDbContext.PaymentMethods.AnyAsync(x => x.UserId == userId), string.Empty);
        }

        public async Task<BaseResult> DeletePaymentMethod(int paymentMethodId, string auth0Id)
        {
            var userId = await GetUserId(auth0Id);
            var result = await _applicationDbContext.PaymentMethods.Where(x => x.Id == paymentMethodId && x.UserId == userId).ExecuteDeleteAsync();
            if (result > 0)
                return new BaseResult(new { }, true, Messages.PaymentMethod_Deleted);
            return new BaseResult(new { }, true, Messages.PaymentMethod_Deleted_Fail);
        }

        public async Task<BaseResult> GetPaymentMethods(string auth0Id)
        {
            var userId = await GetUserId(auth0Id);
            var paymentMethods = await _applicationDbContext.PaymentMethods.Where(x => x.UserId == userId).IncludePaymentMethodDetails().ToListAsync();
            return new BaseResult(paymentMethods, true, string.Empty);
        }

        private async Task<User> GetUser(string auth0Id)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Auth0UserId == auth0Id);
            if (user == null)
                return new User();
            return user;
        }
        public async Task<BaseResult> GetPayments(string auth0Id, long? jobId)
        {
            var userId = await GetUserId(auth0Id);
            var query = _applicationDbContext.Payments.Where(x => x.PaidByUserId == userId || x.PaidToUserId == userId);
            if (jobId.HasValue)
                query = query.Where(x => x.JobId == jobId);
            var payments = await query.ToListAsync();
            return new BaseResult(payments, true, "Payments fetched successfully");
        }
        private async Task<long> GetUserId(string auth0Id)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Auth0UserId == auth0Id);
            if (user == null)
                return 0;
            return user.Id;
        }
    }
}
