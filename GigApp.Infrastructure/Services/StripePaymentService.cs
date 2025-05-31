using Stripe;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Shared;
using GIgApp.Contracts.Responses;

namespace GigApp.Infrastructure.Services
{
    public class StripePaymentService
    {
        private static readonly string SecretKey = "sk_test_51Po2faP8YUqSFuC3kbArrCJooWZHoWwZ2Cs6GvsldvG7XzCcR6ACefOdDzHxtmc8B0BtZaIPa0VELBtS3j7SbARL00K179x9od";

        public StripePaymentService()
        {
            StripeConfiguration.ApiKey = SecretKey;
        }

        public async Task<BaseResult> CreatePaymentMethod(string tokenId)
        {
            try
            {
                var service = new PaymentMethodService();
                var stripePaymentMethod = await service.CreateAsync(new PaymentMethodCreateOptions
                {
                    Type = "card",
                    Card = new PaymentMethodCardOptions
                    {
                        Token = tokenId
                    }
                });

                var paymentMethodEntity = new Domain.Entities.PaymentMethod
                {
                    StripePaymentMethodId = stripePaymentMethod.Id,
                    CardLastFour = stripePaymentMethod.Card.Last4,
                    CardBrand = stripePaymentMethod.Card.Brand,
                    ExpMonth = Convert.ToInt32(stripePaymentMethod.Card.ExpMonth),
                    ExpYear = Convert.ToInt32(stripePaymentMethod.Card.ExpYear),
                    Status = "active",
                    StripeCardId = stripePaymentMethod.Id,
                    CreatedAt = DateTime.UtcNow
                };

                return new BaseResult(paymentMethodEntity, true, Messages.Payment_Method_Added);
            }
            catch (StripeException ex)
            {
                return new BaseResult(new { }, false, ex.Message);
            }
        }
        
        public async Task<BaseResult> ProcessPayment(Payment payment, Domain.Entities.PaymentMethod paymentMethod)
        {
            try
            {
                if (payment.Amount < 0.50m)
                {
                    payment.Status = "failed";
                    payment.FailureReason = "Amount must be at least $0.50";
                    return new BaseResult(payment, false, payment.FailureReason);
                }

                var paymentIntentService = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(payment.Amount * 100),
                    Currency = "usd",
                    PaymentMethod = paymentMethod.StripePaymentMethodId,
                    Description = payment.Description,
                    Confirm = true,
                    PaymentMethodTypes = new List<string> { "card" },
                    CaptureMethod = payment.EscrowStatus == "required" ? "manual" : "automatic",
                    ConfirmationMethod = "automatic",
                    UseStripeSdk = true,
                    PaymentMethodOptions = new PaymentIntentPaymentMethodOptionsOptions
                    {
                        Card = new PaymentIntentPaymentMethodOptionsCardOptions
                        {
                            RequestThreeDSecure = "automatic"
                        }
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "JobId", payment.JobId?.ToString() ?? "" },
                        { "PaymentType", payment.PaymentType },
                        { "PaidByUserId", payment.PaidByUserId?.ToString() ?? "" },
                        { "PaidToUserId", payment.PaidToUserId?.ToString() ?? "" }
                    }
                };

                var paymentIntent = await paymentIntentService.CreateAsync(options);
                payment.StripePaymentIntentId = paymentIntent.Id;
                payment.PaymentMethodId = paymentMethod.Id;
                payment.CreatedAt = DateTime.UtcNow;

                if (paymentIntent.Status == "requires_action" && paymentIntent.NextAction?.Type == "use_stripe_sdk")
                {
                    var confirmResult = await ConfirmPaymentIntent(paymentIntent.Id);
                    if (confirmResult.Status)
                    {
                        return new BaseResult(payment, true, "Payment succeeded");
                    }
                    else
                    {
                        return new BaseResult(payment, false, "Authentication required. Please complete the 3D Secure authentication.");
                    }
                }

                payment.Status = paymentIntent.Status;

                switch (paymentIntent.Status)
                {
                    case "succeeded":
                        payment.Status = "succeeded";
                        return new BaseResult(payment, true, Messages.Payment_Success);

                    case "requires_capture":
                        payment.Status = "pending";
                        payment.EscrowStatus = "held";
                        return new BaseResult(payment, true, "Payment authorized and held in escrow");

                    default:
                        payment.Status = "failed";
                        payment.FailureReason = paymentIntent.LastPaymentError?.Message ?? Messages.Payment_Failed;
                        return new BaseResult(payment, false, payment.FailureReason);
                }
            }
            catch (StripeException ex)
            {
                payment.Status = "failed";
                payment.FailureReason = ex.Message;
                return new BaseResult(payment, false, ex.Message);
            }
        }

        public async Task<BaseResult> CapturePayment(string paymentIntentId, decimal? amount = null)
        {
            try
            {
                var service = new PaymentIntentService();
                var options = new PaymentIntentCaptureOptions();

                if (amount.HasValue)
                {
                    options.AmountToCapture = (long)(amount.Value * 100);
                }

                var paymentIntent = await service.CaptureAsync(paymentIntentId, options);

                return new BaseResult(paymentIntent, true, "Payment captured successfully");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> ConfirmPaymentIntent(string paymentIntentId)
        {
            try
            {
                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.ConfirmAsync(paymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    return new BaseResult(paymentIntent, true, "Payment succeeded");
                }
                else if (paymentIntent.Status == "requires_action")
                {
                    return new BaseResult(paymentIntent, false, "Further action required");
                }
                else
                {
                    return new BaseResult(paymentIntent, false, "Payment failed");
                }
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> CreateCustomer(string paymentMethodId, string email)
        {
            try
            {
                var customerService = new CustomerService();
                var customerOptions = new CustomerCreateOptions
                {
                    Email = email,
                    PaymentMethod = paymentMethodId,
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = paymentMethodId
                    }
                };

                var customer = await customerService.CreateAsync(customerOptions);

                return new BaseResult(customer, true, "Customer created successfully");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> CreatePaymentIntentWithCustomer(string customerId, long amount, string paymentMethodId)
        {
            try
            {
                var paymentIntentService = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Customer = customerId,
                    PaymentMethod = paymentMethodId,
                    PaymentMethodTypes = new List<string> { "card" },
                    Confirm = true,
                    OffSession = true // This is important for charging a customer without their direct interaction
                };

                var paymentIntent = await paymentIntentService.CreateAsync(options);

                if (paymentIntent.Status == "succeeded")
                {
                    return new BaseResult(paymentIntent, true, "Payment succeeded");
                }
                else
                {
                    return new BaseResult(paymentIntent, false, "Payment failed or requires further action");
                }
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> DetachPaymentMethod(string paymentMethodId)
        {
            try
            {
                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = await paymentMethodService.DetachAsync(paymentMethodId);

                return new BaseResult(paymentMethod, true, "Payment method detached successfully");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }
    }
} 