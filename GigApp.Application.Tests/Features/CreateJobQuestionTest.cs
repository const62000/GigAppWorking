using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Notifications;
using GIgApp.Contracts.Requests.JobQuestion;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Responses.Jobs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GigApp.Application.Tests.Features
{
    public class CreateJobQuestionTest
    {
        private readonly Mock<IJobQuestionRepository> _jobQuestionRepositoryMock;
        private readonly Mock<INotificationRepository> _notificationRepositoryMock; 
        private readonly Mock<IValidator<CreateJobQuetion.Command>> _mockValidator;
        private readonly CreateJobQuetion.Handler _handler;
        public CreateJobQuestionTest()
        {
            _jobQuestionRepositoryMock = new Mock<IJobQuestionRepository>();
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _mockValidator = new Mock<IValidator<CreateJobQuetion.Command>>();
            _handler = new CreateJobQuetion.Handler(_jobQuestionRepositoryMock.Object, _mockValidator.Object, _notificationRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var command = new CreateJobQuetion.Command
            {
                JobId = 4,
                 Title = "Test Question",
                 CurrentTimeStamp = DateTime.UtcNow,    
            };

            var jobQuestionMap = new CreateJobQuestionRequest
            (
                command.JobId,
                command.Title,
                command.CurrentTimeStamp
            );

            var expectedResult = new CreateJobQuestionResult(4, true);

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            _jobQuestionRepositoryMock.Setup(repo => repo.CreateJobQuestionAsync(jobQuestionMap)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Xunit.Assert.True(actualResult.Status);
        }

        [Fact]
        public async Task Handle_RepositoryFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new CreateJobQuetion.Command
            {
                JobId = 4,
                Title = "Test Question",
                CurrentTimeStamp = DateTime.UtcNow,
            };

            var jobQuestionMap = new CreateJobQuestionRequest
            (
                command.JobId,
                command.Title,
                command.CurrentTimeStamp
            );

            var expectedResult = new CreateJobQuestionResult(4, false); // Simulate failure result

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            _jobQuestionRepositoryMock.Setup(repo => repo.CreateJobQuestionAsync(jobQuestionMap)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
        }


    }
}
