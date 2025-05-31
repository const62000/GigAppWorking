using Stripe;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Requests.BankAccount;
using GigApp.Domain.Entities;
using GigApp.Application.Interfaces.Payments;
using System.Threading.Tasks;
using System.Collections.Generic;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Shared;
using Stripe.FinancialConnections;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GigApp.Application.Interfaces.Notifications;

namespace GigApp.Infrastructure.Repositories.Payments
{
    public class StripePaymentRepository : IStripePaymentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly INotificationRepository _notificationRepository;
        private static readonly string SecretKey = "sk_test_51QgrriP8D5DYTUU6DWtfBAqW5IIwAW2NaGmynAZsXImcWdNkCYkeddbEqUGbEWIYOuTJ9kqqQhaZtrUgkHK44ZuD00T6TKIjaw";


        public StripePaymentRepository(ApplicationDbContext applicationDbContext, INotificationRepository notificationRepository)
        {
            _applicationDbContext = applicationDbContext;
            _notificationRepository = notificationRepository;
            StripeConfiguration.ApiKey = SecretKey;
        }

        public async Task<BaseResult> NotifySenderAndReceiver(Event @event)
        {
            switch (@event.Type)
            {
                case "checkout.session.completed":
                    var session = @event.Data.Object as Stripe.Checkout.Session;
                    // Access session.SetupIntentId or session.PaymentIntentId
                    // Handle storing PaymentMethod, marking as complete, etc.
                    break;

                case "payment_intent.succeeded":
                    var paymentIntent = @event.Data.Object as PaymentIntent;
                    var cutomerId = paymentIntent?.CustomerId ?? "cus_Rap6Z9SnOCiPpb";
                    var accountId = paymentIntent?.TransferData.DestinationId ?? "acct_1QhFel00WXxdx09W";
                    var sender = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.StripeCustomerId == cutomerId);
                    if(sender != null)
                    {
                        await _notificationRepository.ReceivePaymentSuccessNotification(sender.Id);
                    }
                    var receiver = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.StripeAccountId == accountId);
                    if(receiver != null)
                    {
                        await _notificationRepository.ReceivePaymentSuccessNotification(receiver.Id);
                    }
                    // Mark the timesheet as "paid" in your DB
                    break;

                case "payment_intent.payment_failed":
                    var failedIntent = @event.Data.Object as PaymentIntent;
                    // Notify Job Provider to update payment info
                    break;

                default:
                    // Handle other event types as needed
                    break;
            }
            return new BaseResult(null, true, "Payment succeeded");
        }
        public void CreateTestPayment()
        {
            try
            {
                var chargeService = new ChargeService();
                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = 1000, // Amount in cents
                    Currency = "usd",
                    Source = "tok_visa",  // Test card number (simulates a successful payment)
                };

                var charge = chargeService.Create(chargeOptions);
                Console.WriteLine($"Charge created: {charge.Id}");
            }
            catch (StripeException ex)
            {
            }
        }

        public async Task<BaseResult> CreateBankAccountToken(string RoutingNumber, string AccountNumber, string UserName)
        {
            try
            {
                var tokenService = new TokenService();

                var bankAccountOptions = new TokenCreateOptions
                {
                    BankAccount = new TokenBankAccountOptions
                    {
                        Country = "US", // Country code (e.g., "US")
                        Currency = "usd", // Currency (e.g., "usd")
                        AccountHolderName = UserName, // Account holder's name
                        AccountHolderType = "individual", // "individual" or "company"
                        RoutingNumber = RoutingNumber, // Routing number
                        AccountNumber = AccountNumber, // Bank account number
                    }
                };

                Token bankToken = await tokenService.CreateAsync(bankAccountOptions);

                return new BaseResult( bankToken.Id, true, "Bank account token created successfully");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
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

                var paymentMethodEntity = new GigApp.Domain.Entities.PaymentMethod
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

                return new BaseResult(paymentMethodEntity, true, string.Empty);
            }
            catch (StripeException ex)
            {
                return new BaseResult(new { }, false, ex.Message);
            }
        }
        public async Task<BaseResult> AttachPaymentMethodToCustomer(string customerId, string paymentMethodId)
        {
            try
            {
                // Create a PaymentMethodAttachOptions object
                var attachOptions = new PaymentMethodAttachOptions
                {
                    Customer = customerId,
                };

                // Attach the payment method to the customer
                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = await paymentMethodService.AttachAsync(paymentMethodId, attachOptions);
                return new BaseResult(new {}, true, "Payment method attached to customer successfully");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }
        public async Task<BaseResult> ProcessPayment(GigApp.Domain.Entities.Payment payment, GigApp.Domain.Entities.PaymentMethod paymentMethod,string customerId, string accountId)
        {
            try
            {
                //var result = await CheckAccountPayoutStatus(accountId);
                //if (!result.Status)
                //{
                //    result = await CreateStripeAccountLink(accountId);
                //    return result;
                //}
                if (payment.Amount < 0.50m)
                {
                    payment.Status = "failed";
                    payment.FailureReason = "Amount must be at least $0.50";
                    return new BaseResult(payment, false, payment.FailureReason);
                }
                var amount = (long)(payment.Amount * 100);
                var paymentIntentService = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethod = paymentMethod.StripePaymentMethodId,
                    Customer = customerId,
                    Description = payment.Description,
                    Confirm = true,
                    PaymentMethodTypes = new List<string> { "card" },
                    CaptureMethod = "automatic",
                    ConfirmationMethod = "automatic",
                    TransferData = new PaymentIntentTransferDataOptions
                    {
                        Destination = accountId,
                        Amount = amount
                    },
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
                payment.StripeTransferId = paymentIntent.TransferGroup;
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
                        return new BaseResult(payment, true, string.Empty);

                    case "requires_capture":
                        payment.Status = "pending";
                        payment.EscrowStatus = "held";
                        return new BaseResult(payment, true, "Payment authorized and held in escrow");

                    default:
                        payment.Status = "failed";
                        payment.FailureReason = paymentIntent.LastPaymentError?.Message ?? string.Empty;
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

                return new BaseResult(customer.Id, true, "Customer created successfully");
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
                    OffSession = true
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

        // public async Task<BaseResult> InitiateBankAccountVerification(string paymentMethodId)
        // {
        //     try
        //     {
        //         var paymentMethodService = new PaymentMethodService();
        //         var paymentMethod = await paymentMethodService.VerifyAsync(
        //             paymentMethodId,
        //             new PaymentMethodVerifyOptions
        //             {
        //                 Amounts = null // Stripe determines the deposit amounts
        //             }
        //         );

        //         return new BaseResult(paymentMethod, true, "Verification initiated successfully");
        //     }
        //     catch (StripeException ex)
        //     {
        //         return new BaseResult(null, false, ex.Message);
        //     }
        // }

        public async Task<BaseResult> CreateCustomerWithBankAccount(string bankToken, User user)
        {
            try
            {
                var tokenService = new TokenService();
                var token = tokenService.Get(bankToken);
                var result = await CreateStripeAccount(user.Email, "none");
                if(!result.Status)
                {
                    return new BaseResult(null,false,result.Message);
                }
                var stripeAccountId = result.Data as string;
                result = await AttachBankAccountToConnectedAccount(stripeAccountId,bankToken);
                if(result.Status)
                {
                    return new BaseResult(new StripeBankAccount(bankToken,stripeAccountId),true,Messages.BankAccount_Created);
                }
                else
                {
                    return new BaseResult(null,false,Messages.BankAccount_Created_Fail);
                }
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }
        public async Task<BaseResult> VerifyBankAccount(string bankAccountId , string customerId,List<long?> amounts)
        {
            try
            {
                var options = new CustomerPaymentSourceVerifyOptions
                {
                    Amounts = amounts,
                };
                var service = new CustomerPaymentSourceService();
                var bankAccount = service.Verify(customerId, bankAccountId, options);
                if(bankAccount.Id != null)
                {
                    return new BaseResult(new StripeBankAccount(bankAccountId,customerId),true,Messages.BankAccount_Verified);
                }
                else
                {
                    return new BaseResult(new{},false,Messages.BankAccount_Verified_Fail);
                }   
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> CreateEscrowPaymentIntent(string customerId, long amount, string paymentMethodId)
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
                    CaptureMethod = "manual",
                    OffSession = true
                };

                var paymentIntent = await paymentIntentService.CreateAsync(options);

                if (paymentIntent.Status == "requires_capture")
                {
                    return new BaseResult(paymentIntent, true, "Payment authorized and held in escrow");
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

        public async Task<BaseResult> ReleaseEscrowPayment(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.CaptureAsync(paymentIntentId);

                return new BaseResult(paymentIntent, true, "Payment captured and released from escrow");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> SendPayoutToBankAccount(string connectedAccountId, long amount, string currency = "usd")
        {
            try
            {
                var result = await CheckAccountPayoutStatus(connectedAccountId);
                if(!result.Status)
                {
                    result = await CreateStripeAccountLink(connectedAccountId);
                    return result;
                }
                var payoutOptions = new PayoutCreateOptions
                {
                    Amount = amount, // Amount in cents (e.g., $50.00)
                    Currency = currency,
                    Description = connectedAccountId
                };

                var payoutService = new PayoutService();
                var payout = payoutService.Create(payoutOptions);


                return new BaseResult(payout.Id, true, "Payout successful");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> AttachBankAccountToConnectedAccount(string connectedAccountId,string bankToken)
        {
            try
            {// This would come from the client side

                var bankAccountOptions = new AccountExternalAccountCreateOptions
                {
                    ExternalAccount = bankToken // The token generated from the client-side
                };

                var bankAccountService = new AccountExternalAccountService();
                var bankAccount = bankAccountService.Create(connectedAccountId, bankAccountOptions);
                return new BaseResult(bankAccount, true, "Payment method attached successfully");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }
        public async Task<BaseResult> CreateStripeAccount(string email, string type = "standard")
        {
            try
            {
                var options = new AccountCreateOptions
                {
                    Email = email,
                    Type = type
                };
                var service = new Stripe.AccountService();
                var account = await service.CreateAsync(options);
                return new BaseResult(account.Id, true, "Stripe account created successfully");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> CreateStripeAccountLink(string accountId)
        {
            try
            {
                var service = new AccountLinkService();
                var link = service.Create(new AccountLinkCreateOptions
                {
                Account = accountId,
                RefreshUrl = "https://docs.stripe.com/api/account_links/create",
                ReturnUrl = "https://docs.stripe.com/api/account_links/create",
                    Type = "account_onboarding"
                });
                return new BaseResult(link.Url,true,string.Empty);
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }

        public async Task<BaseResult> GetStripeAccountBalance(string accountId)
        {
            var balanceService = new BalanceService();
            var balance = balanceService.Get(new RequestOptions { StripeAccount = accountId});
            var x = balance.Available[0].Amount;
            return new BaseResult(balance, true, "Stripe account balance retrieved successfully");
        }
        public async Task<BaseResult> GetStripeAccount(string accountId)
        {
            try
            {
                List<string> requirements = new List<string>();
                var accountService = new Stripe.AccountService();
                var account = accountService.Get(accountId); // Replace with your connected account ID

                foreach (var field in account.Requirements.CurrentlyDue)
                {
                    requirements.Add(field);
                }

                foreach (var field in account.Requirements.PastDue)
                {
                    requirements.Add(field);
                }
                return new BaseResult(requirements, true, "Stripe account retrieved successfully");
            }
            catch (StripeException ex)
            {
                return new BaseResult(null, false, ex.Message);
            }
        }
        public async Task<BaseResult> CheckAccountPayoutStatus(string accountId)
        {
            var service = new Stripe.AccountService();
            var account = service.Get(accountId);
            return new BaseResult(new {}, account.PayoutsEnabled , $"{account.Requirements?.DisabledReason ?? string.Empty}");
        }
    }
} 