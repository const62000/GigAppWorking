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
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.UpdateJob;

namespace GigApp.Application.Tests.Features
{
    public class UpdateJobTest
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IValidator<Command>> _mockValidator;
        private readonly UpdateJob.Handler _handler;
        public UpdateJobTest()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _mockValidator = new Mock<IValidator<Command>>();
            _handler = new UpdateJob.Handler(_jobRepositoryMock.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var command = new UpdateJob.Command
            {
                JobId = 5,
                Title = "Software Developer",
                Description = "Develop software applications...",
                Requirements = "Experience with Dart",
                Date = DateOnly.Parse("2023-10-01"),
                Time = TimeOnly.Parse("09:00:00"),
                Rate = decimal.Parse("50.00"),
                Status = "open",
            };

            var updateJobMap = new UpdateJobRequest
            (
                command.JobId,
                command.Title,
                command.Description,
                command.Requirements,
                command.Date,
                command.Time,
                command.Rate,
                command.Status,
                0,
                0,
                DateTime.Now,
                DateTime.Now
            );

            var expectedResult = new BaseResult(new { Success = true }, true, "Job data updated successfully!");

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            _jobRepositoryMock.Setup(repo => repo.UpdateJobAsync(updateJobMap)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Xunit.Assert.True(actualResult.Status);

            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);
        }

        [Fact]
        public async Task Handle_InvalidJobId_ReturnsFailureResult()
        {
            // Arrange
            var command = new UpdateJob.Command
            {
                JobId = 999, // Assuming this ID does not exist
                Title = "Software Developer Updated",
                Description = "Develop software applications...",
                Requirements = "Experience with Dart",
                Date = DateOnly.Parse("2023-10-01"),
                Time = TimeOnly.Parse("09:00:00"),
                Rate = decimal.Parse("50.00"),
                Status = "open",
            };
            var expectedMessage = "Job with ID 999 not found.";
            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());

            var updateJobMap = new UpdateJobRequest
            (
                command.JobId,
                command.Title,
                command.Description,
                command.Requirements,
                command.Date,
                command.Time,
                command.Rate,
                command.Status,
                0,
                0,
                DateTime.Now,
                DateTime.Now
            );

            _jobRepositoryMock.Setup(repo => repo.UpdateJobAsync(updateJobMap))
                .ReturnsAsync(new BaseResult(new { }, false, $"Job with ID {command.JobId} not found."));

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Equal(expectedMessage, actualResult.Message);
        }

    }
}
