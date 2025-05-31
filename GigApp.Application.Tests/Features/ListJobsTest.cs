using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.CQRS.Implementations.Jobs.Queries;
using GigApp.Application.Interfaces.Jobs;
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
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.ListJobs;

namespace GigApp.Application.Tests.Features
{
    public class ListJobsTest
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IValidator<Command>> _mockValidator;
        private readonly ListJobs.Handler _handler;
        public ListJobsTest()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _mockValidator = new Mock<IValidator<Command>>();
            _handler = new ListJobs.Handler(_jobRepositoryMock.Object, _mockValidator.Object);
        }
        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {

            // Arrange
            var command = new ListJobs.Command
            {
                Page = 1,
                PerPage = 10,
                SearchQuery = "developer",
                Filters = new Dictionary<string, string>
                {
                    { "status", "open" }
                },
                SortBy = "date",
                Ascending = true,
            };

            // Map Command to JobSearchRequest
            var jobSearchRequest = new JobSearchRequest(
                command.Page,
                command.PerPage,
                command.SearchQuery,
                command.Filters,
                command.SortBy,
                command.Ascending,
                0,
                0,
                0
            );
            var expectedjobList = new List<Job>
            {
                new Job
                {
                    Id = 5,
                    Title = "Software Developer",
                    Description = "Develop software applications...",
                    Requirements = "Experience with Dart",
                    Date = DateOnly.Parse("2023-10-01"),
                    Time = TimeOnly.Parse("09:00:00"),
                    Rate = decimal.Parse("50.00"),
                    Status = "open",
                }
            };

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            //_jobRepositoryMock.Setup(repo => repo.ListJobsAsync(jobSearchRequest)).ReturnsAsync(expectedjobList);

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.True(actualResult.Status);
            Xunit.Assert.NotNull(actualResult.Data);
            var jobList = (List<JobDetailsResult>)actualResult.Data;
            Xunit.Assert.Single(jobList);
        }
        [Fact]
        public async Task Handle_NoJobsFound_ReturnsEmptyListResult()
        {
            // Arrange
            var command = new ListJobs.Command
            {
                Page = 1,
                PerPage = 10,
                SearchQuery = "developer",
                Filters = new Dictionary<string, string>
                {
                    { "status", "closed" } // Assuming no jobs are in this status
                },
                SortBy = "date",
                Ascending = true,
            };

            // Map Command to JobSearchRequest
            var jobSearchRequest = new JobSearchRequest(
                command.Page,
                command.PerPage,
                command.SearchQuery,
                command.Filters,
                command.SortBy,
                command.Ascending,
                0,0,0
            );

            _mockValidator.Setup(v => v.Validate(command)).Returns(new ValidationResult());
            //_jobRepositoryMock.Setup(repo => repo.ListJobsAsync(jobSearchRequest)).ReturnsAsync(new List<Job>()); // Return an empty list

            // Act
            var actualResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Xunit.Assert.True(actualResult.Status); // You can still have a success status
            Xunit.Assert.NotNull(actualResult.Data);
            var jobList = (List<JobDetailsResult>)actualResult.Data;
            Xunit.Assert.Empty(jobList); // Check that the job list is empty
        }



    }
}
