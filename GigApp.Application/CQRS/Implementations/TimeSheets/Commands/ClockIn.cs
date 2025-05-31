

using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Mapster;

namespace GigApp.Application.CQRS.Implementations.TimeSheets.Commands
{
    public class ClockIn
    {
        public class Command : ICommand
        {
            public string Auth0Id { get; set; } = string.Empty;
            public int HiringId {  get; set; }
            public string Note { get;set; } = string.Empty;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.HiringId).NotEqual(0);
                RuleFor(x => x.Auth0Id).NotEmpty();
            }
        }

        public class Handler(ITimeSheetRepository _timeSheetRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var timeSheet = request.Adapt<TimeSheet>();
                timeSheet.TimeSheetNotes = request.Note;
                var result = await _timeSheetRepository.ClockInHiredJob(timeSheet,request.Auth0Id);
                return result;
            }
        }
    }
}
