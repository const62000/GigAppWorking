namespace GigApp.Contracts.Requests.TimeSheet;

public class TimeSheetLocationRequest
{
    public long TimeSheetId { get; set; }
    public DateTime LocationCapturedDateTime { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}