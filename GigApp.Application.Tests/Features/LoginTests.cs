
using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Auth.Commands;
using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using Moq;
using Xunit;
using Assert = Xunit.Assert; // Use static to avoid ambiguity

namespace GigApp.Application.Tests.Features;

public class LoginTests
{
    private readonly Mock<IAuthRepository> _mockAuthRepository;
    private readonly Mock<IUserDeviceRepository> _mockUserDeviceRepository;
    private readonly Mock<IValidator<Login.Query>> _mockValidator;
    private readonly Login.Handler _handler;

    public LoginTests()
    {
        _mockAuthRepository = new Mock<IAuthRepository>();
        _mockUserDeviceRepository = new Mock<IUserDeviceRepository>();
        _mockValidator = new Mock<IValidator<Login.Query>>();
        _handler = new Login.Handler(_mockAuthRepository.Object, _mockValidator.Object,_mockUserDeviceRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsSuccessResult()
    {
        // Arrange
        var query = new Login.Query { Email = "testuser", Password = "testpassword" };
        var expectedResult = new BaseResult(new { Success = true }, true, string.Empty);

        _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
        _mockAuthRepository.Setup(repo => repo.Login(query.Email, query.Password)).ReturnsAsync(expectedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal(expectedResult.Data, result.Data);
        Assert.Equal(expectedResult.Message, result.Message);
    }

    [Fact]
    public async Task Handle_InvalidQuery_ReturnsFailureResult()
    {
        // Arrange
        var query = new Login.Query { Email = "", Password = "" }; // Invalid query
        var validationFailure = new ValidationFailure("UserName", "Username is required");
        var validationResult = new ValidationResult(new[] { validationFailure });

        _mockValidator.Setup(v => v.Validate(query)).Returns(validationResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.Status);
        Assert.Equal("Username is required", result.Message);
    }
}
