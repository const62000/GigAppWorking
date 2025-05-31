using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.AssignManager;

namespace GigApp.Application.Tests.Features
{
    public class AssignJobManagerTest
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IValidator<Command>> _mockValidator;
        private readonly Handler _handler;
        public AssignJobManagerTest() 
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _mockValidator = new Mock<IValidator<Command>>();
            _handler = new Handler(_jobRepositoryMock.Object, _mockValidator.Object);
        }
        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            //Arrange
            var command = new Command
            {
                JobId = 123,
                Auth0Id = string.Empty,
                UserId = 42,               
            };

            var request = new AssignManagerRequest(
                command.JobId,
                command.Auth0Id,
                command.UserId
            );

            var expectedResult = new BaseResult(new { Success = true }, true, "Job manager assigned successfully");
            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            _jobRepositoryMock.Setup(repo => repo.AssignJobManagerAsync(request)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Xunit.Assert.True(actualResult.Status);

            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);
        }

        [Fact]
        public async Task Handle_InvalidJobOrUser_ReturnsFailureResult()
        {
            // Arrange
            var command = new AssignManager.Command
            {
                JobId = 999, // Assume this JobId does not exist
                Auth0Id = string.Empty,
                UserId = 42
            };

            var request = new AssignManagerRequest(
                command.JobId,
                command.Auth0Id,
                command.UserId
            );

            var expectedResult = new BaseResult(new {}, false, "No jobs or user found.");

            // Setup validation to succeed
            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());

            // Setup repository to return a failure result for the non-existing job
            _jobRepositoryMock
                .Setup(repo => repo.AssignJobManagerAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Equal("No jobs or user found.", actualResult.Message);
        }

    }

}
