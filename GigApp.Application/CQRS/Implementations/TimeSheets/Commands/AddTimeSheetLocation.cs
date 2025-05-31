using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using Mapster;
using MediatR;

namespace GigApp.Application.CQRS.Implementations.TimeSheets.Commands;

public class AddTimeSheetLocation
{
    public class Command : ICommand
    {
        public long TimeSheetId { get; set; }
        public DateTime LocationCapturedDateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; } 
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.TimeSheetId).NotEqual(0);
            RuleFor(x => x.LocationCapturedDateTime).NotEmpty();
            RuleFor(x => x.Latitude).NotEmpty();
            RuleFor(x => x.Longitude).NotEmpty();
        }
    }

    public class Handler(IValidator<Command> _validator,ITimeSheetRepository _timeSheetRepository) : ICommandHandler<Command>
    {
        public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
            }
            var timeSheetLocation = request.Adapt<TimeSheetLocation>();
            var result = await _timeSheetRepository.AddTimeSheetLocation(timeSheetLocation);
            return result;
        }
    }
}