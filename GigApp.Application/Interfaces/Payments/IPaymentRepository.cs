using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Payments
{
    public interface IPaymentRepository
    {
        Task<BaseResult> AddPaymentMethod(PaymentMethod paymentMethod,string auth0Id);
        Task<BaseResult> DeletePaymentMethod(int paymentMethodId,string auth0Id);
        Task<BaseResult> GetPaymentMethods(string auth0Id);
        Task<BaseResult> CheckPaymentMethod(string auth0Id);
        Task<BaseResult> GetPayments(string auth0Id, long? jobId);
    }
}
