using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.DeleteJob;

namespace GigApp.Application.Tests.Features
{
    public class DeleteJobTest
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IValidator<Query>> _mockValidator;
        private readonly Handler _handler;

        public DeleteJobTest()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _mockValidator = new Mock<IValidator<Query>>();
            _handler = new Handler(_jobRepositoryMock.Object, _mockValidator.Object);           
        }
        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResult()
        {
            var query = new Query { JobId = 7 };
            long jobId = (long)query.JobId;

            var expectedResult = new BaseResult(new { Success = true }, true, "Job deleted successfully!");
            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());

            _jobRepositoryMock.Setup(repo => repo.DeleteJobAsync(jobId)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.True(actualResult.Status);
            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);
        }

        [Fact]
        public async Task Handle_InvalidQuery_ReturnsValidationError()
        {
            var query = new Query { JobId = 0 };

            // Use the actual error message defined in the validator
            var expectedErrorMessage = "JobId must be a valid/positive number.";
            var validationResult = new ValidationResult(new[] { new ValidationFailure("JobId", expectedErrorMessage) });

            _mockValidator.Setup(v => v.Validate(query)).Returns(validationResult);

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Equal(expectedErrorMessage, actualResult.Message);
        }

        [Fact]
        public async Task Handle_JobNotFound_ReturnsNotFoundMessage()
        {
            var query = new Query { JobId = 999 }; // Job ID that doesn't exist
            long jobId = (long)query.JobId;

            var expectedResult = new BaseResult(new {}, false, $"Job with ID {jobId} not found.");
            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());

            // Setup the repository to return a not found result
            _jobRepositoryMock.Setup(repo => repo.DeleteJobAsync(jobId)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);
        }


    }
}
