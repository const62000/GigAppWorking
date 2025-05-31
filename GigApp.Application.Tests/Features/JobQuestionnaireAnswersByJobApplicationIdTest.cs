using FluentValidation;
using FluentValidation.Results;
using GigApp.Application.CQRS.Implementations.Jobs.Queries;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Domain.Entities;
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
    public class JobQuestionnaireAnswersByJobApplicationIdTest 
    {
        private readonly Mock<IJobQuestionRepository> _jobRepositoryMock;
        private readonly Mock<IValidator<JobQuestionnaireAnswersByJobApplicationId.Query>> _mockValidator;
        private readonly JobQuestionnaireAnswersByJobApplicationId.Handler _handler;

        public JobQuestionnaireAnswersByJobApplicationIdTest() 
        {
            _jobRepositoryMock = new Mock<IJobQuestionRepository>();
            _mockValidator = new Mock<IValidator<JobQuestionnaireAnswersByJobApplicationId.Query>>();
            _handler = new JobQuestionnaireAnswersByJobApplicationId.Handler(_jobRepositoryMock.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResult()
        {
            var query = new JobQuestionnaireAnswersByJobApplicationId.Query { JobApplicationId = 20 };
            long jobApplicationId = query.JobApplicationId;


            var expectedjobQuestions = new List<JobQuestionnaireAnswer>
            {
                new JobQuestionnaireAnswer
                {
                    Id = 8,
                    JobApplicationId = 20,
                    UserId =4,
                    Answer = "Test",
                }
            };

            _mockValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
            _jobRepositoryMock.Setup(repo => repo.GetQuestionaireAnswersByJobApplicationIdAsync(jobApplicationId)).ReturnsAsync(expectedjobQuestions);


            var actualresult = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Xunit.Assert.True(actualresult.Status);
            Xunit.Assert.NotNull(actualresult.Data);
        }

        [Fact]
        public async Task Handle_InvalidQuery_ReturnsFailureResult()
        {
            // Arrange
            var query = new JobQuestionnaireAnswersByJobApplicationId.Query { JobApplicationId = 0 }; // Invalid ID
            var validationResult = new ValidationResult(new[] { new ValidationFailure("JobApplicationId", "JobApplicationId must be greater than zero.") });

            _mockValidator.Setup(v => v.Validate(query)).Returns(validationResult);

            // Act
            var actualResult = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Xunit.Assert.False(actualResult.Status);
        }

    }
}
