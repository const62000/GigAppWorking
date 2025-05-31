using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Queries;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Domain.Entities;
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
    public class JobsByStatusTest
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IValidator<JobsByStatus.Query>> _mockValidator;
        private readonly JobsByStatus.Handler _handler;

        public JobsByStatusTest()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _mockValidator = new Mock<IValidator<JobsByStatus.Query>>();
            _handler = new JobsByStatus.Handler(_jobRepositoryMock.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResult()
        {
            var query = new JobsByStatus.Query { Status = "open"};
            string status = query.Status;

            var expectedjobList = new List<Job>
            {
                new Job
                {
                    Id = 5,
                    Title = "Updated Title",
                    Status = "open",
                }
            };

            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());

            _jobRepositoryMock.Setup(repo => repo.GetJobsByStatusAsync(status)).ReturnsAsync(expectedjobList);

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);
            var expectedJob = expectedjobList.First();

            // Assert
            Xunit.Assert.True(actualResult.Status);
            Xunit.Assert.NotNull(actualResult.Data);
        }

        [Fact]
        public async Task Handle_NoJobsFound_ReturnsEmptyList()
        {
            // Arrange
            var query = new JobsByStatus.Query { Status = "closed" }; // Assuming no jobs with this status
            var status = query.Status;

            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
            _jobRepositoryMock.Setup(repo => repo.GetJobsByStatusAsync(status)).ReturnsAsync(new List<Job>()); // No jobs

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.True(actualResult.Status); // Should be true as the operation was successful
            Xunit.Assert.NotNull(actualResult.Data);
            // Cast Data to the expected type
            var jobDetailResults = actualResult.Data as IEnumerable<JobStatusResult>;

            // Assert that the data is empty
            Xunit.Assert.Empty(jobDetailResults!);
        }
    }

}
