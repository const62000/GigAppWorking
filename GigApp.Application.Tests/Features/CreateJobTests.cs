using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.Interfaces.Auth;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Notifications;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.CreateJob;

namespace GigApp.Application.Tests.Features
{
    public class CreateJobTests
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly Mock<IValidator<Command>> _mockValidator;
        private readonly CreateJob.Handler _handler;
        public CreateJobTests()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _mockValidator = new Mock<IValidator<Command>>();
            _handler = new CreateJob.Handler(_jobRepositoryMock.Object, _mockValidator.Object,_notificationRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var command = new CreateJob.Command
            {
                Title = "Software Developer",
                Description = "Develop software applications...",
                Requirements = "Experience with Dart",
                Date = DateOnly.Parse("2023-10-01"),
                Time = TimeOnly.Parse("09:00:00"),
                Rate = decimal.Parse("50.00"),
                Status = "open",
                AddressId = 1,
                FacilityId = 1
            };

            var jobMap = new CreateJobRequest
            (
                command.Title,
                command.Description,
                command.LicenseRequirments,
                command.Requirements,
                command.Date,
                command.Time,
                command.Rate,
                command.Status,
                command.AddressId,
                command.FacilityId,
                0,
                0,
                1,
                DateTime.Now,
                DateTime.Now
            );

            var expectedResult = new CreateJobResult(1, "Job created successfully!");

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            _jobRepositoryMock.Setup(repo => repo.CreateJobAsync(jobMap,"adcadc")).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Xunit.Assert.True(actualResult.Status);

            Xunit.Assert.Equal(expectedResult.message, actualResult.Message);
        }
         
        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFailureResult()
        {
            // Arrange
            var command = new CreateJob.Command
            {
                Title = "", 
                Description = "Develop software applications...",
                Requirements = "Experience with Dart",
                Date = DateOnly.Parse("2023-10-01"),
                Time = TimeOnly.Parse("09:00:00"),
                Rate = decimal.Parse("50.00"),
                Status = "open",
            };

            // Set up validation to return a failure
            var validationFailure = new ValidationResult(new[]
            {
                new ValidationFailure("Title", "Title cannot be empty.")
            });

            _mockValidator.Setup(v => v.Validate(command)).Returns(validationFailure);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status); 
            Xunit.Assert.Contains("Title cannot be empty.", actualResult.Message);
        }
        [Fact]
        public async Task Handle_InvalidRate_ReturnsFailureResult()
        {
            // Arrange
            var command = new CreateJob.Command
            {
                Title = "Software Developer", // Valid title
                Description = "Develop software applications...",
                Requirements = "Experience with Dart",
                Date = DateOnly.Parse("2023-10-01"),
                Time = TimeOnly.Parse("09:00:00"),
                Rate = decimal.Parse("-50.00"), // Invalid rate
                Status = "open",
            };

            // Set up validation to return a failure for the Rate
            var validationFailure = new ValidationResult(new[]
            {
                new ValidationFailure("Rate", "Rate must be a positive number.")
            });

            _mockValidator.Setup(v => v.Validate(command)).Returns(validationFailure);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Contains("Rate must be a positive number.", actualResult.Message);
        }

    }
}
