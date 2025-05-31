using GigApp.Application.Interfaces.Jobs;
using GIgApp.Contracts.Responses;
using MediatR;

namespace GigApp.Application.CQRS.Implementations.TimeSheets.Queries;

public class GetTimeSheetLocationByTimeSheet
{
    public class Query : IRequest<BaseResult>
    {
        public long TimeSheetId { get; set; }
    }

    internal sealed class Handler(ITimeSheetRepository _timeSheetRepository) : IRequestHandler<Query, BaseResult>
    {
        public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _timeSheetRepository.GetTimeSheetLocationByTimeSheetId(request.TimeSheetId);
            return result;
        }
    }
}