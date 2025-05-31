using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using System.Threading.Tasks;
using Stripe;

namespace GigApp.Application.Interfaces.Payments
{
    public interface IStripePaymentRepository
    {
        Task<BaseResult> CreatePaymentMethod(string tokenId);
        Task<BaseResult> ProcessPayment(Payment payment, GigApp.Domain.Entities.PaymentMethod paymentMethod,string customerId, string accountId);
        Task<BaseResult> CapturePayment(string paymentIntentId, decimal? amount = null);
        Task<BaseResult> ConfirmPaymentIntent(string paymentIntentId);
        Task<BaseResult> CreateCustomer(string paymentMethodId, string email);
        Task<BaseResult> AttachPaymentMethodToCustomer(string customerId, string paymentMethodId);
        Task<BaseResult> CreatePaymentIntentWithCustomer(string customerId, long amount, string paymentMethodId);
        Task<BaseResult> DetachPaymentMethod(string paymentMethodId);
        Task<BaseResult> CreateCustomerWithBankAccount(string bankToken, User user);
        Task<BaseResult> VerifyBankAccount(string bankAccountId, string customerId, List<long?> amounts);
        Task<BaseResult> CreateEscrowPaymentIntent(string customerId, long amount, string paymentMethodId);
        Task<BaseResult> ReleaseEscrowPayment(string paymentIntentId);
        Task<BaseResult> SendPayoutToBankAccount(string connectedAccountId, long amount, string currency = "usd");
        Task<BaseResult> CreateBankAccountToken(string RoutingNumber, string AccountNumber, string UserName);
        Task<BaseResult> CreateStripeAccount(string email, string type = "standard");
        Task<BaseResult> CreateStripeAccountLink(string accountId);
        Task<BaseResult> NotifySenderAndReceiver(Event @event);
    }
}