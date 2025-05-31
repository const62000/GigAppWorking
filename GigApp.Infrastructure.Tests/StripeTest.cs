using Xunit;
using GigApp.Infrastructure.Repositories.Payments;
using GigApp.Domain.Entities;
using Stripe;
using GIgApp.Contracts.Shared;
using Xunit.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GigApp.Infrastructure.Database;
using GigApp.Application.Interfaces.Notifications;

namespace GigApp.Infrastructure.Tests
{
    public class StripePaymentRepositoryTests
    {
        private readonly StripePaymentRepository _stripeRepository;
        private readonly ITestOutputHelper _output;
        private readonly Mock<PayoutService> _mockPayoutService;
        private readonly Mock<AccountService> _mockAccountService;
        private readonly Mock<AccountExternalAccountService> _mockAccountExternalAccountService;
        private readonly Mock<ApplicationDbContext> _mockApplicationDbContext;
        private readonly Mock<INotificationRepository> _mockNotificationRepository;

        public StripePaymentRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
            _mockApplicationDbContext = new Mock<ApplicationDbContext>();
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _stripeRepository = new StripePaymentRepository(_mockApplicationDbContext.Object, _mockNotificationRepository.Object);
            _mockPayoutService = new Mock<PayoutService>();
            _mockAccountService = new Mock<AccountService>();
            _mockAccountExternalAccountService = new Mock<AccountExternalAccountService>();
        }

        // [Fact]
        // public async Task CreateCustomerAndProcessPayment_WithValidData_ShouldSucceed()
        // {
        //     // Arrange
        //     var tokenId = "tok_visa"; // Use a test token to create a new payment method
        //     var email = "abdulrahman.ahmed.abdulah@gmail.com";

        //     // Act: Create a new Payment Method
        //     var paymentMethodResult = await _stripeRepository.CreatePaymentMethod(tokenId);
        //     Assert.True(paymentMethodResult.Status, $"Failed to create payment method: {paymentMethodResult.Message}");
        //     var paymentMethod = paymentMethodResult.Data as GigApp.Domain.Entities.PaymentMethod;
        //     Assert.NotNull(paymentMethod);

        //     // Act: Create Customer
        //     var customerResult = await _stripeRepository.CreateCustomer(paymentMethod.StripePaymentMethodId, email);
        //     Assert.True(customerResult.Status, $"Failed to create customer: {customerResult.Message}");

        //     // Act: Create PaymentIntent with Customer and Payment Method
        //     var paymentResult = await _stripeRepository.CreatePaymentIntentWithCustomer(customerResult.Data as string, 10000, paymentMethod.StripePaymentMethodId);
        //     _output.WriteLine($"Result: {paymentResult.Status}, Message: {paymentResult.Message}");

        //     // Assert
        //     Assert.True(paymentResult.Status, $"Failed to process payment: {paymentResult.Message}");
        //     var paymentIntent = paymentResult.Data as PaymentIntent;
        //     Assert.NotNull(paymentIntent);
        //     Assert.Equal("succeeded", paymentIntent.Status);
        // }

        //[Fact]
        //public async Task CreateCustomerWithBankAccount_WithValidData_ShouldSucceed()
        //{
        //    // Arrange
        //    var bankToken = "btok_1QebTJP6LCCZIe8YJKcSTeKW"; // Use a test bank token
        //    var email = "abdulrahman.ahmeed.abdulah@gmail.com";
        //    var firstName = "Abdulrahman";

        //    // Act: Create Customer with Bank Account
        //    var customerResult = await _stripeRepository.CreateCustomerWithBankAccount(bankToken, email, firstName);
        //    Assert.True(customerResult.Status, $"Failed to create customer with bank account: {customerResult.Message}");

        //    // Assert
        //    var customerId = customerResult.Data as string;
        //    Assert.NotNull(customerId);
        //    _output.WriteLine($"Customer ID: {customerId}");
        //}

        // [Fact]
        // public async Task CaptureEscrowPayment_WithValidData_ShouldSucceed()
        // {
        //     // Arrange
        //     var paymentIntentId = "pi_1QbTH9P8YUqSFuC3Xu8LsAZA"; // Use a test payment intent ID

        //     // Act: Capture the payment
        //     var captureResult = await _stripeRepository.CapturePayment(paymentIntentId);

        //     // Assert
        //     Assert.True(captureResult.Status, $"Failed to capture payment: {captureResult.Message}");
        //     var paymentIntent = captureResult.Data as PaymentIntent;
        //     Assert.NotNull(paymentIntent);
        //     Assert.Equal("succeeded", paymentIntent.Status);
        // }

        // [Fact]
        // public async Task CreateEscrowPaymentIntent_WithValidData_ShouldSucceed()
        // {
        //     // Arrange
        //     var customerId = "cus_RUMQS4EbQDQcrg"; // Use a test customer ID
        //     var amount = 10000; // Amount in cents
        //     var paymentMethodId = "pm_1QbNhPP8YUqSFuC34SwBbrjy"; // Use a test payment method ID

        //     // Act: Create Escrow Payment Intent
        //     var result = await _stripeRepository.CreateEscrowPaymentIntent(customerId, amount, paymentMethodId);

        //     // Assert
        //     Assert.True(result.Status, $"Failed to create escrow payment intent: {result.Message}");
        //     var paymentIntent = result.Data as PaymentIntent;
        //     Assert.NotNull(paymentIntent);
        //     Assert.Equal("requires_capture", paymentIntent.Status);
        //     result = await _stripeRepository.ReleaseEscrowPayment(paymentIntent.Id);
        //     Assert.True(result.Status, $"Failed to release escrow payment: {result.Message}");
        // }

        //[Fact]
        //public async Task SendPayoutToBankAccount_WithValidData_ShouldSucceed()
        //{
        //    // Arrange
        //    var connectedAccountId = "acct_1QfjzP01iRDDC4R5"; // Use a test connected account ID
        //    var amount = 10000; // Amount in cents
        //    var currency = "usd";

        //    var payout = new Payout
        //    {
        //        Id = "po_1QbNhPP8YUqSFuC34SwBbrjy",
        //        Amount = amount,
        //        Currency = currency,
        //        Status = "paid"
        //    };

        //    _mockPayoutService
        //        .Setup(service => service.CreateAsync(It.IsAny<PayoutCreateOptions>(), null, default))
        //        .ReturnsAsync(payout);

        //    // Act
        //    var result = await _stripeRepository.SendPayoutToBankAccount(connectedAccountId, amount, currency);

        //    // Assert
        //    Assert.True(result.Status, $"Failed to send payout: {result.Message}");
        //    Assert.NotNull(result.Data);
        //    Assert.Equal("Payout successful", result.Message);
        //}

        [Fact]
        public async Task CreateStripeAccount_WithValidData_ShouldSucceed()
        {
            // Arrange
            var email = "abdulrahman.abdulah@gmail.com";
            var account = new Account
            {
                Id = "acct_1QbNhPP8YUqSFuC34SwBbrjy",
                Email = email
            };

            _mockAccountService
                .Setup(service => service.CreateAsync(It.IsAny<AccountCreateOptions>(), null, default))
                .ReturnsAsync(account);

            // Act
            var result = await _stripeRepository.CreateStripeAccount(email);

            // Assert
            Assert.True(result.Status, $"Failed to create Stripe account: {result.Message}");
            Assert.NotNull(result.Data);
            Assert.Equal("Stripe account created successfully", result.Message);
        }

        // [Fact]
        // public async Task AttachBankAccountToConnectedAccount_WithValidData_ShouldSucceed()
        // {
        //     // Arrange
        //     var connectedAccountId = "acct_1QdGAeQJn7CPAXwO";
        //     var bankToken = "btok_1QbTH9P8YUqSFuC3Xu8LsAZA";
        //     var bankAccount = new Stripe.BankAccount
        //     {
        //         Id = "ba_1QdC71P8YUqSFuC3Ney5lVf9"
        //     };

        //     _mockAccountExternalAccountService
        //         .Setup(service => service.CreateAsync(connectedAccountId, It.IsAny<AccountExternalAccountCreateOptions>(), null, default))
        //         .ReturnsAsync(bankAccount);

        //     // Act
        //     var result = await _stripeRepository.AttachBankAccountToConnectedAccount(connectedAccountId, bankToken);

        //     // Assert
        //     Assert.True(result.Status, $"Failed to attach bank account: {result.Message}");
        //     Assert.NotNull(result.Data);
        //     Assert.Equal("Bank account attached successfully", result.Message);
        // }

        //[Fact]
        //public async Task GetStripeAccount_WithValidAccountId_ShouldReturnRequirements()
        //{
        //   // Arrange
        //   var accountId = "acct_1QejGe08veNbm9QC";

        //   // Act
        //   var result = await _stripeRepository.GetStripeAccount(accountId); 

        //   // Assert
        //   Assert.True(result.Status, $"Failed to retrieve account: {result.Message}");
        //   var requirements = result.Data as List<string>;
        //   Assert.NotNull(requirements);
        //   Assert.Contains("field1", requirements);
        //   Assert.Contains("field2", requirements);
        //   Assert.Contains("field3", requirements);
        //}

        //[Fact]
        //public async Task CreateStripeAccountLink_WithValidAccountId_ShouldReturnLink()
        //{
        //    // Arrange
        //    var accountId = "acct_1QfpQPP1KKbHshnp"; // Use a test account ID
        //    var expectedUrl = "https://connect.stripe.com/setup/s/some_link"; // Mocked expected URL

        //    var mockAccountLinkService = new Mock<AccountLinkService>();
        //    mockAccountLinkService
        //        .Setup(service => service.Create(It.IsAny<AccountLinkCreateOptions>(), null))
        //        .Returns(new AccountLink { Url = expectedUrl });


        //    // Act
        //    var result = await _stripeRepository.CreateStripeAccountLink(accountId);

        //    // Assert
        //    Assert.True(result.Status, $"Failed to create account link: {result.Message}");
        //    Assert.NotNull(result.Data);
        //    Assert.Equal(expectedUrl, result.Data);
        //}

        // [Fact]
        // public async Task GetStripeAccountBalance_WithValidAccountId_ShouldReturnBalance()
        // {
        //     // Arrange
        //     var accountId = "acct_1QfpQPP1KKbHshnp"; // Use a test account ID
        //     var expectedBalance = new Balance
        //     {
        //         Available = new List<BalanceAmount>
        //         {
        //             new BalanceAmount { Amount = 10000, Currency = "usd" }
        //         }
        //     };

        //     var mockBalanceService = new Mock<BalanceService>();
        //     mockBalanceService
        //         .Setup(service => service.Get(It.IsAny<RequestOptions>()))
        //         .Returns(expectedBalance);

        //     _stripeRepository.CreateTestPayment();

        //     // Act
        //     var result = await _stripeRepository.GetStripeAccountBalance(accountId);

        //     // Assert
        //     Assert.True(result.Status, $"Failed to retrieve balance: {result.Message}");
        //     Assert.NotNull(result.Data);
        //     var balance = result.Data as Balance;
        //     Assert.Equal(expectedBalance.Available[0].Amount, balance.Available[0].Amount);
        //     Assert.Equal(expectedBalance.Available[0].Currency, balance.Available[0].Currency);
        // }
    }
}
