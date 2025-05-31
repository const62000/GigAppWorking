
using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Auth.Commands;
using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using MapsterMapper;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace GigApp.Application.Tests.Features
{
    public class SignupTest
    {
        private readonly Mock<IAuthRepository> _mockAuthRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidator<SignUp.Command>> _mockValidator;
        private readonly Mock<IMapper> _mapper;
        private readonly SignUp.Handler _handler;

        public SignupTest()
        {
            _mapper = new Mock<IMapper>();
            _mockAuthRepository = new Mock<IAuthRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidator = new Mock<IValidator<SignUp.Command>>();
            _handler = new SignUp.Handler(_mockAuthRepository.Object,_mockUserRepository.Object, _mockValidator.Object,_mapper.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResult()
        {
            // Arrange
            var command = new SignUp.Command { FirstName = "testuser", LastName = "testuser", Email = "aboddodc" ,Password = "testpassword" };
            var expectedResult = new BaseResult(new { Success = true }, true, string.Empty);

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            _mockAuthRepository.Setup(repo => repo.SignUp(command.Email, command.Password)).ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Status);
            Assert.Equal(expectedResult.Data, result.Data);
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task Handle_InvalidQuery_ReturnsFailureResult()
        {
            // Arrange
            var query = new SignUp.Command { FirstName = "",LastName="",Email = "", Password = "" }; // Invalid query
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
}
