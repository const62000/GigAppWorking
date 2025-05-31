using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.JobDetails;

namespace GigApp.Application.Tests.Features
{
    public class JobDetailsTest
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IValidator<Query>> _mockValidator;
        private readonly JobDetails.Handler _handler;
        public JobDetailsTest()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _mockValidator = new Mock<IValidator<Query>>();
            _handler = new JobDetails.Handler(_jobRepositoryMock.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResult()
        {
            // Arrange
            var query = new JobDetails.Query { JobId = 5 };
            long jobId = (long)query.JobId;

            var jobDetails = new Job // Ensure this is of the correct type
            {
                Id = jobId,
                Title = "Software Developer",
                Description = "Develop software applications...",
                Requirements = "Experience with Dart",
                Date = DateOnly.Parse("2023-10-01"),
                Time = TimeOnly.Parse("09:00:00"),
                Rate = decimal.Parse("50.00"),
                Status = "open"
            };

            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
            _jobRepositoryMock.Setup(repo => repo.GetJobDetailsAsync(jobId)).ReturnsAsync(jobDetails);

            var expectedResult = new BaseResult(new { Success = true }, true, "Job details fetched successfully!");

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.True(actualResult.Status);

            // Extract JobId from the actual result
            var actualJobDetails = (JobDetailsResult)actualResult.Data;
            Xunit.Assert.Equal(jobId, actualJobDetails.Id);
            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);

        }


        [Fact]
        public async Task Handle_JobNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var query = new JobDetails.Query { JobId = 15 }; // Assume this JobId doesn't exist
            long jobId = query.JobId;

            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
            // Set up the mock to return null when the job is not found
            _jobRepositoryMock.Setup(repo => repo.GetJobDetailsAsync(jobId))
                              .ReturnsAsync((Job)null); 

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status); 
            Xunit.Assert.Equal($"Job with ID {jobId} not found.", actualResult.Message);
        }


    }
}
