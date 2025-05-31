using Xunit;
using Moq;
using GigApp.Infrastructure.Repositories.Users;
using GigApp.Domain.Entities;
using GigApp.Application.Interfaces.Users;
using GigApp.Application.Interfaces.Payments;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Requests.BankAccount;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class BankAccountRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _mockDbContext;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IStripePaymentRepository> _mockStripePaymentRepository;
    private readonly BankAccountRepository _bankAccountRepository;

    public BankAccountRepositoryTests()
    {
        _mockDbContext = new Mock<ApplicationDbContext>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockStripePaymentRepository = new Mock<IStripePaymentRepository>();
        _bankAccountRepository = new BankAccountRepository(_mockDbContext.Object, _mockUserRepository.Object, _mockStripePaymentRepository.Object);
    }

    [Fact]
    public async Task AddBankAccount_WithValidData_ShouldSucceed()
    {
        // Arrange
        var bankAccount = new BankAccount
        {
            BankName = "Test Bank",
            BankAccountType = "Checking",
            BankAccountNumber = "123456789",
            BankAccountName = "Test Account",
            BankSwiftCode = "TSTB123",
            BankCountry = "US",
            Status = "active"
        };

        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe"
        };

        var bankToken = "test_bank_token";

        _mockUserRepository.Setup(repo => repo.GetUserId(It.IsAny<string>())).ReturnsAsync(user.Id);
        _mockStripePaymentRepository.Setup(repo => repo.CreateBankAccountToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new BaseResult(bankToken, true, "Token created successfully"));
        _mockStripePaymentRepository.Setup(repo => repo.CreateCustomerWithBankAccount(It.IsAny<string>(), It.IsAny<User>()))
            .ReturnsAsync(new BaseResult(new StripeBankAccount("bank_account_id", "customer_id"), true, "Customer created successfully"));
        _mockDbContext.Setup(db => db.BankAccounts.AddAsync(It.IsAny<BankAccount>(), default))
            .Returns(new ValueTask<EntityEntry<BankAccount>>(new Mock<EntityEntry<BankAccount>>().Object));
        _mockDbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _bankAccountRepository.AddBankAccount(bankAccount, "auth0Id", bankToken);

        // Assert
        Assert.True(result.Status, $"Failed to add bank account: {result.Message}");
        Assert.NotNull(bankAccount.StripeCustomerId);
        Assert.NotNull(bankAccount.StripeBankAccountId);
    }
}