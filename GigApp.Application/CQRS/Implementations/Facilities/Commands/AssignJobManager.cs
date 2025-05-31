using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Facilities;
using GIgApp.Contracts.Responses;

namespace GigApp.Application.CQRS.Implementations.Facilities.Commands
{
    public class AssignJobManager
    {
        public class Command : ICommand
        {
            public long ClientId { get; set; }
            public string Email { get; set; }
        }
        internal sealed class Handler(IFacilitiesRepository _facilitiesRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _facilitiesRepository.AddJobManager(request.ClientId, request.Email);
                return result;
            }
        }
    }
}