using GigApp.Domain.Entities;
using GIgApp.Contracts.Shared;

namespace VMS.Client.Repositories.Abstractions
{
    public interface ITimeSheetRepository
    {
        Task<List<TimeSheet>> GetTimeSheetsAsync();
        Task<TimeSheet> GetTimeSheetByHiredIdAsync(int hiredId);
        Task<bool> ClockInAsync(TimeSheet timeSheet);
        Task<bool> ClockOutAsync(TimeSheet timeSheet);
    }
} 