using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Payments
{
    public interface IDisputeRepository
    {
        Task<BaseResult> RaiseDispute(Dispute dispute, string auth0Id);
        Task<BaseResult> ProcessDispute(DisputeAction disputeAction, string auth0Id);
        Task<BaseResult> GetDisputeById(long id);
        Task<BaseResult> GetDisputeByUserId(string auth0Id);
    }
}
