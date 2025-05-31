using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class AddAnswer
    {
        public class Command : ICommand
        {
            public string Auth0Id { get; set; } = string.Empty;
            public long? JobApplicationId { get; set; }

            public long? QuestionId { get; set; }
            public string Answer { get; set; } = string.Empty;
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator() 
            {
                RuleFor(x => x.QuestionId).NotEqual(0);
                RuleFor(x => x.JobApplicationId).NotEqual(0);
                RuleFor(x => x.Answer).NotEmpty();
                RuleFor(x => x.Auth0Id).NotEmpty();
            }
        }
        internal sealed class Handler(IJobQuestionRepository _jobQuestionRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _jobQuestionRepository.AddAnswer(request.Adapt<JobQuestionnaireAnswer>(),request.Auth0Id);
                return result;
            }
        }
    }
}
