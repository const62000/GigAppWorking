using FluentValidation;
using GigApp.Application.CQRS.Implementations.Jobs.Queries;
using GigApp.Application.CQRS.Implementations.LicenseInfo.Queries;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GigApp.Application.Tests.Features
{
    public class FreelancerLicenseInfoTest
    {
        private readonly Mock<IFreelancerRepository> _freelancerRepositoryMock; 
        private readonly Mock<IValidator<GetFreelancerLicenseInfo.Query>> _mockValidator;
        private readonly GetFreelancerLicenseInfo.Handler _handler;

        public FreelancerLicenseInfoTest()
        {
            _freelancerRepositoryMock = new Mock<IFreelancerRepository>();
            _mockValidator = new Mock<IValidator<GetFreelancerLicenseInfo.Query>>();
            _handler = new GetFreelancerLicenseInfo.Handler(_freelancerRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetFreelancerLicenseInfo.Query { FreelancerUserId = 4 };
            long userId = (long)query.FreelancerUserId;

            var freelancerLicenseInfo = new List<FreelancerLicense> // Ensure this is of the correct type
            {
                new FreelancerLicense
                {
                    Id = 4,
                    FreelancerUserId = 4,
                    LicenseName = "string",
                    LicenseStatus = "Not Approved",
                }
            };

            _freelancerRepositoryMock.Setup(repo => repo.GetFreelancerLicenseInfoAsync(userId)).ReturnsAsync(freelancerLicenseInfo);

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            var expectedResult = new BaseResult(freelancerLicenseInfo, true, "Freelancers License details fetched successfully!");

            // Assert
            Xunit.Assert.True(actualResult.Status);
            Xunit.Assert.Equal(expectedResult.Message, actualResult.Message);
        }

        [Fact]
        public async Task Handle_FreelancerRepositoryThrowsException_ReturnsFailureResult()
        {
            // Arrange
            var query = new GetFreelancerLicenseInfo.Query { FreelancerUserId = 4 };
            long userId = (long)query.FreelancerUserId;

            // Set up the mock to throw an exception when the repository method is called
            _freelancerRepositoryMock.Setup(repo => repo.GetFreelancerLicenseInfoAsync(userId))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            // The actual result should indicate failure due to exception
            Xunit.Assert.False(actualResult.Status);
            Xunit.Assert.Equal("An error occurred while fetching License details: Database connection failed", actualResult.Message);
        }


    }
}
