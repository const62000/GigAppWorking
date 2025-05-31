using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Queries;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GigApp.Application.Tests.Features
{
    public class JobQuestionByJobIdTest
    {
        private readonly Mock<IJobQuestionRepository> _jobRepositoryMock;
        private readonly Mock<IValidator<JobQuestionsById.Query>> _mockValidator;
        private readonly JobQuestionsById.Handler _handler;

        public JobQuestionByJobIdTest()
        {
            _jobRepositoryMock = new Mock<IJobQuestionRepository>();
            _mockValidator = new Mock<IValidator<JobQuestionsById.Query>>();
            _handler = new JobQuestionsById.Handler(_jobRepositoryMock.Object, _mockValidator.Object);
        }
        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResult()
        {
            var query = new JobQuestionsById.Query {  JobId = 1};
            long jobId = query.JobId;


            var expectedjobQuestions = new List<JobQuestionnaire>
            {
                new JobQuestionnaire
                {
                    Id = 2,
                    JobId = 4,
                    Question = "Hello test",
                }
            };

            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
            _jobRepositoryMock.Setup(repo => repo.GetJobQuestionsById(jobId)).ReturnsAsync(expectedjobQuestions);


            var actualresult = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Xunit.Assert.True(actualresult.Status);
            Xunit.Assert.NotNull(actualresult.Data);
        }
        [Fact]
        public async Task Handle_InvalidQuery_ReturnsFailureResult()
        {
            // Arrange
            var query = new JobQuestionsById.Query { JobId = 0 }; // Invalid JobId
            var validationResult = new ValidationResult(new[] { new ValidationFailure("JobId", "JobId must be greater than zero.") });

            _mockValidator.Setup(v => v.Validate(query)).Returns(validationResult);

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status); // Assuming your result has this property
        }

    }
}
