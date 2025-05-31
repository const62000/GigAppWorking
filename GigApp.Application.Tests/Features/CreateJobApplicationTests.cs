using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
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
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.CreateJobApplication;

namespace GigApp.Application.Tests.Features
{
    public class CreateJobApplicationTests
    {
        private readonly Mock<IJobApplicationRepository> _jobApplicationRepositoryMock;
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly Mock<IValidator<Command>> _mockValidator;
        private readonly CreateJobApplication.Handler _handler;
        public CreateJobApplicationTests()
        {
            _jobApplicationRepositoryMock = new Mock<IJobApplicationRepository>();
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _mockValidator = new Mock<IValidator<Command>>();
            _handler = new Handler(_jobApplicationRepositoryMock.Object,_notificationRepositoryMock.Object, _mockValidator.Object);
        }
        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var command = new Command
            {
                 JobId = 4,
                 FreelancerUserId = 4,
                 Proposal = "I am interested in this job...",
                 ProposedHourlyRate = decimal.Parse("50.00"),
            };

            var jobMap = new CreateJobApplicationRequest
            (
                command.JobId,
                command.FreelancerUserId,
                command.Proposal,
                command.ProposedHourlyRate,
                command.Answers
            );

            var expectedResult = new CreateJobApplicationResult(0, "Application submitted successfully");

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            //_jobApplicationRepositoryMock.Setup(repo => repo.CreateJobApplicationAsync(jobMap)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Xunit.Assert.True(actualResult.Status);

            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFailureResult()
        {
            // Arrange
            var command = new Command
            {
                JobId = 4,
                FreelancerUserId = 4,
                Proposal = "I am interested in this job...",
                ProposedHourlyRate = decimal.Parse("50.00"),
            };

            // Simulate validation failure
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("FreelancerUserId", "Freelancer user does not exist.")
            });

            _mockValidator.Setup(v => v.Validate(command)).Returns(validationResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Equal("Freelancer user does not exist.", actualResult.Message);
        }


    }
}
