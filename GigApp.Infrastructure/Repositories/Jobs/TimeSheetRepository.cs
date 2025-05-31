using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using GigApp.Infrastructure.Repositories.Payments;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using GigApp.Contracts.Enums;
using System.Security.Cryptography;
using GigApp.Application.Interfaces.Payments;

namespace GigApp.Infrastructure.Repositories.Jobs
{
    internal class TimeSheetRepository(ApplicationDbContext _applicationDbContext,IStripePaymentRepository _stripePaymentRepository) : ITimeSheetRepository
    {
        public async Task<BaseResult> GetTimeSheetById(long id)
        {
            var timeSheet = await _applicationDbContext.TimeSheets.IncludeTimeSheetDetails().FirstOrDefaultAsync(x => x.Id == id);
            return new BaseResult(timeSheet!,true,string.Empty); 
        }
        public async Task<BaseResult> ClockInHiredJob(TimeSheet timeSheet, string auth0Id)
        {
            if(await _applicationDbContext.TimeSheets.AnyAsync(x=>x.HiringId == timeSheet.HiringId && x.Status == TimeSheetStatus.Active.ToString())) 
            {
                return new BaseResult(new { }, false, Messages.TimeSheet_ClockIn_Fail);
            }
            else
            {
                var user = await _applicationDbContext.Users.Where(x => x.Auth0UserId == auth0Id).FirstOrDefaultAsync();
                if(user == null) 
                    return new BaseResult(new {},false,Messages.User_Not_Found);
                timeSheet.UserId = user.Id;
                await _applicationDbContext.TimeSheets.AddAsync(timeSheet);
                await _applicationDbContext.SaveChangesAsync();
                return new BaseResult(new {timeSheet.Id},true,Messages.TimeSheet_ClockIn);
            }
        }

        public async Task<BaseResult> AddTimeSheetLocation(TimeSheetLocation timeSheetLocation)
        {
            await _applicationDbContext.TimeSheetLocations.AddAsync(timeSheetLocation);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new {timeSheetLocation.Id},true,Messages.TimeSheet_Location_Added);
        }

        public async Task<BaseResult> GetTimeSheetLocationByTimeSheetId(long timeSheetId)
        {
            var timeSheetLocation = await _applicationDbContext.TimeSheetLocations.Where(x => x.TimeSheetId == timeSheetId).ToListAsync();
            return new BaseResult(timeSheetLocation, true, string.Empty);
        }

        public async Task<BaseResult> ChangeTimeSheetStatus(long timeSheetId, TimeSheetStatus status, string auth0Id)
        {
            await _applicationDbContext.TimeSheets.Where(x => x.Id == timeSheetId).ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, status.ToString()));
            return new BaseResult(new { timeSheetId }, true, Messages.TimeSheet_Status_Changed);
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);
                for (int i = 0; i < random.Length; i++)
                {
                    random[i] = chars[data[i] % chars.Length];
                }
            }
            return new string(random);
        }

        public async Task<BaseResult> ChangeTimeSheetApprovalStatus(long timeSheetId, TimeSheetApprovalStatus approvalStatus, string auth0Id)
        {
            await _applicationDbContext.TimeSheets.Where(x => x.Id == timeSheetId).ExecuteUpdateAsync(s => s.SetProperty(x => x.TimeSheetApprovalStatus, approvalStatus.ToString()));
            var timeSheet = await _applicationDbContext.TimeSheets.Where(x => x.Id == timeSheetId).Include(x => x.Hiring).ThenInclude(x => x.Job).FirstOrDefaultAsync();
            var time = timeSheet.ClockOut - timeSheet.ClockIn;
            var amount = (decimal)((time?.Minutes ?? 0) / 60.0) * timeSheet.Hiring.Job.Rate;
            var paymentMethod = await _applicationDbContext.PaymentMethods.Where(x => x.UserId == timeSheet.Hiring.HiredManagerId).OrderByDescending(x=>x.CreatedAt).FirstOrDefaultAsync();
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == timeSheet.Hiring.FreelancerId);
            var manager = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == timeSheet.Hiring.HiredManagerId);
            if(paymentMethod == null || user == null)
                return new BaseResult(new {},false,Messages.PaymentMethod_Not_Found);
            var payment = new Payment
            {
                Amount = amount,
                PaymentMethodId = paymentMethod.Id,
                PaymentType = "Stripe",
                Description = "Payment for time sheet " + timeSheetId,
                PaidByUserId = timeSheet.Hiring.HiredManagerId,
                JobId = timeSheet.Hiring.JobId,
                PaidToUserId = timeSheet.Hiring.FreelancerId
            };
            var result = await _stripePaymentRepository.ProcessPayment(payment,paymentMethod,manager.StripeCustomerId,user.StripeAccountId); 
            if(result.Status)
            {
                payment = result.Data as Payment;
                //var payoutResult = await _stripePaymentRepository.SendPayoutToBankAccount(bankAccount.StripeCustomerId,(long)amount);    
                //if(!payoutResult.Status)
                //{
                //    return payoutResult;
                //}
                //payment.StripeTransferId = payoutResult.Data.ToString();
                payment.Status = "Paid";
                await _applicationDbContext.Payments.AddAsync(payment);
                await _applicationDbContext.SaveChangesAsync();
                return new BaseResult(new { timeSheetId }, true, Messages.TimeSheet_Approval_Status_Changed);
            }
            else
            {
                return result;
            }
        }

        public async Task<BaseResult> ClockOutHiredJob(TimeSheet timeSheet, string auth0Id)
        {
            if (await _applicationDbContext.TimeSheets.AnyAsync(x => x.HiringId == timeSheet.HiringId && x.Status != TimeSheetStatus.Completed.ToString()))
            {
                var user = await _applicationDbContext.Users.Where(x => x.Auth0UserId == auth0Id).FirstOrDefaultAsync();
                if (user == null)
                    return new BaseResult(new { }, false, Messages.User_Not_Found);
                timeSheet.UserId = user.Id;
                await _applicationDbContext.TimeSheets.Where(x => x.HiringId == timeSheet.HiringId && x.Status != TimeSheetStatus.Completed.ToString()).ExecuteUpdateAsync(s =>
                s.SetProperty(x => x.Status, TimeSheetStatus.Completed.ToString())
                .SetProperty(x => x.TimeSheetNotes, timeSheet.TimeSheetNotes ?? "Completed")
                .SetProperty(x => x.ClockOut, DateTime.UtcNow));
                await _applicationDbContext.SaveChangesAsync();
                return new BaseResult(new { timeSheet.Id }, true, Messages.TimeSheet_ClockOut);
            }
            else
            {
                return new BaseResult(new { }, false, Messages.TimeSheet_ClockOut_Fail);
            }
        }

        public async Task<BaseResult> GetTimeSheetByHiredId(int hiredId)
        {
            var timeSheet = await _applicationDbContext.TimeSheets.IncludeTimeSheetDetails().Where(x => x.HiringId == hiredId).ToListAsync();
            return new BaseResult(timeSheet!, true, string.Empty);
        }

        public async Task<BaseResult> GetTimeSheetByUserId(string auth0Id)
        {
            if(string.IsNullOrEmpty(auth0Id))
                return new BaseResult(new {},false,string.Empty);
            var timeSheets = await _applicationDbContext.TimeSheets.IncludeTimeSheetDetails().Where(x => x.User.Auth0UserId == auth0Id).ToListAsync();
            return new BaseResult(timeSheets!, true, string.Empty);
        }
    }
}
