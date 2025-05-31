using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Notifications;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GigApp.Application.Tests.Features
{
    public class JobwithdrawalTest
    {
        private readonly Mock<IJobApplicationRepository> _jobApplicationRepositoryMock;
        private readonly Mock<IValidator<JobWithdrawal.Command>> _mockValidator;
        private readonly JobWithdrawal.Handler _handler;
        public JobwithdrawalTest()
        {
            _jobApplicationRepositoryMock = new Mock<IJobApplicationRepository>();
            _mockValidator = new Mock<IValidator<JobWithdrawal.Command>>();
            _handler = new JobWithdrawal.Handler(_jobApplicationRepositoryMock.Object, _mockValidator.Object);
        }
        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var command = new JobWithdrawal.Command
            {
                JobApplicationId = 4,
                WithdrawalReason = "Test Withdrawal"
            };

            var requestMap = new JobWithdrawalRequest
            (
                command.JobApplicationId,
                command.WithdrawalReason
            );
            
            var expectedResult = new BaseResult(new { }, true, string.Empty);
            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            _jobApplicationRepositoryMock.Setup(repo => repo.WithdrawalJobAsync(requestMap)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.True(actualResult.Status);
        }

        [Fact]
        public async Task Handle_RepositoryFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new JobWithdrawal.Command
            {
                JobApplicationId = 4,
                WithdrawalReason = "Test Withdrawal"
            };

            var requestMap = new JobWithdrawalRequest
            (
                command.JobApplicationId,
                command.WithdrawalReason
            );

            var expectedResult = new BaseResult(new { }, false, "Job application not found.");

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());  // Simulate successful validation
            _jobApplicationRepositoryMock.Setup(repo => repo.WithdrawalJobAsync(requestMap)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Equal("Job application not found.", actualResult.Message);
        }

    }
}
