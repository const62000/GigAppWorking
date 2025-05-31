namespace GigApp.Contracts.Enums;

public enum TimeSheetStatus
{
    Active,
    Inactive,
    Completed,
    Cancelled,
    Disputed,
    InReview
}

public enum TimeSheetApprovalStatus
{
    Pending,
    Approved,
    Rejected
}
