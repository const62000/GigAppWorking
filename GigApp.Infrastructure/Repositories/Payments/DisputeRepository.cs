using GigApp.Application.Interfaces.Payments;
using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace GigApp.Infrastructure.Repositories.Payments
{
    internal class DisputeRepository(JsonSerializerOptions options, ApplicationDbContext _applicationDbContext, IUserRepository _userRepository) : IDisputeRepository
    {
        public async Task<BaseResult> RaiseDispute(Dispute dispute, string auth0Id)
        {
            // var userId = await _userRepository.GetUserId(auth0Id);
            // dispute.ProcessedByUserId = userId;
            // await _applicationDbContext.AddAsync(dispute);
            // await _applicationDbContext.SaveChangesAsync();
            // return new BaseResult(dispute, true, Messages.Dispute_Added);
            await Task.CompletedTask;
            return new BaseResult(new { }, true, string.Empty);
        }

        public async Task<BaseResult> ProcessDispute(DisputeAction disputeAction, string auth0Id)
        {
            // var userId = await _userRepository.GetUserId(auth0Id);
            // disputeAction.ProcessedByUserId = userId;
            // await _applicationDbContext.DisputeActions.AddAsync(disputeAction);
            // await _applicationDbContext.SaveChangesAsync();
            // return new BaseResult(disputeAction, true, Messages.Dispute_Process_Added);
            await Task.CompletedTask;
            return new BaseResult(new { }, true, string.Empty);
        }
        public async Task<BaseResult> GetDisputeById(long id)
        {
            await Task.CompletedTask;
            // var dispute = await _applicationDbContext.Disputes.Include(x => x.DisputeActions).FirstOrDefaultAsync(x => x.Id == id);
            // var disputeJson = JsonSerializer.Serialize(dispute, options);
            // return new BaseResult(disputeJson ?? string.Empty, true, string.Empty);
            return new BaseResult(new { }, true, string.Empty);
        }
        public async Task<BaseResult> GetDisputeByUserId(string auth0Id)
        {
            await Task.CompletedTask;
            // var disputes = await _applicationDbContext.Disputes.Include(x => x.DisputeActions).Where(x=> x.ProcessedByUser.Auth0UserId == auth0Id).ToListAsync();
            // var disputesJson = JsonSerializer.Serialize(disputes, options);
            // return new BaseResult(disputesJson, true, string.Empty);
            return new BaseResult(new { }, true, string.Empty);
        }
    }
}

