using GigApp.Application.Interfaces.Jobs;
using MediatR;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using GigApp.Contracts.Enums;
namespace GigApp.Application.CQRS.Implementations.TimeSheets.Commands;

public class ChangeTimeSheetApprovalStatus 
{
    public class Command : ICommand
    {
        public long TimeSheetId { get; set; }
        public TimeSheetApprovalStatus ApprovalStatus { get; set; }
        public string Auth0Id { get; set; }
    }
    public class Handler : ICommandHandler<Command>
    {
        private readonly ITimeSheetRepository _timeSheetRepository;
        public Handler(ITimeSheetRepository timeSheetRepository)
        {
            _timeSheetRepository = timeSheetRepository;
        }
        public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _timeSheetRepository.ChangeTimeSheetApprovalStatus(request.TimeSheetId, request.ApprovalStatus, request.Auth0Id);
        }
    }
}