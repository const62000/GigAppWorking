using System;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Responses;
using GigApp.Contracts.Enums;

namespace GigApp.Application.Interfaces.Jobs;

public interface ITimeSheetRepository
{
    Task<BaseResult> ClockInHiredJob(TimeSheet timeSheet,string auth0Id);
    Task<BaseResult> ClockOutHiredJob(TimeSheet timeSheet, string auth0Id);
    Task<BaseResult> GetTimeSheetById(long id);
    Task<BaseResult> GetTimeSheetByHiredId(int hiredId);
    Task<BaseResult> GetTimeSheetByUserId(string auth0Id);
    Task<BaseResult> AddTimeSheetLocation(TimeSheetLocation timeSheetLocation);
    Task<BaseResult> GetTimeSheetLocationByTimeSheetId(long timeSheetId);
    Task<BaseResult> ChangeTimeSheetStatus(long timeSheetId, TimeSheetStatus status, string auth0Id);
    Task<BaseResult> ChangeTimeSheetApprovalStatus(long timeSheetId, TimeSheetApprovalStatus approvalStatus, string auth0Id);
}