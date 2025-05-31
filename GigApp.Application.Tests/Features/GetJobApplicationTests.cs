using FluentValidation;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.CQRS.Implementations.Jobs.Queries;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static GigApp.Application.CQRS.Implementations.Jobs.Queries.GetJobApplication;
using FluentValidation.Results;

namespace GigApp.Application.Tests.Features
{
    public class GetJobApplicationTests
    {
        private readonly Mock<IJobApplicationRepository> _jobApplicationRepositoryMock;
        private readonly Mock<IValidator<Query>> _mockValidator;
        private readonly GetJobApplication.Handler _handler;
        public GetJobApplicationTests()
        {
            _jobApplicationRepositoryMock = new Mock<IJobApplicationRepository>();
            _mockValidator = new Mock<IValidator<Query>>();
            _handler = new Handler(_jobApplicationRepositoryMock.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResult()
        {
            // Arrange
            var query = new Query { JobId = 4 };
            long jobId = (long)query.JobId;

            var jobApplicationData = new List<JobApplication> // Ensure this is of the correct type
            {
                new JobApplication
                {
                    Id = 3,
                    FreelancerUserId = 4,
                    JobApplicationStatus = "open"
                }
            };

            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
            _jobApplicationRepositoryMock.Setup(repo => repo.GetJobApplicationById(jobId)).ReturnsAsync(jobApplicationData);

            var expectedResult = new BaseResult(new { Success = true }, true, "Job application data retrieved successfully.");

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.True(actualResult.Status);
            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);
        }

        [Fact]
        public async Task Handle_ValidationFails_ReturnsValidationError()
        {
            // Arrange
            var query = new Query { JobId = 0 }; // Invalid JobId
            var validationResult = new ValidationResult(new[] { new ValidationFailure("JobId", "JobId must be a valid job ID") });

            // Setup the mock validator to return invalid for this test
            _mockValidator.Setup(v => v.Validate(query)).Returns(validationResult);

            var expectedResult = new BaseResult(new {}, false, validationResult.Errors.First().ErrorMessage);

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);
        }
    }
}
