using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GigApp.Contracts.Enums;

namespace GIgApp.Contracts.Requests.Jobs
{
    public record CreateJobRequest(
        string Title, 
        string Description,
        string? LicenseRequirments, 
        string? Requirements, 
        DateOnly Date, 
        TimeOnly Time, 
        decimal Rate, 
        string Status,
        long AddressId,
        long FacilityId,
        JobType JobType,
        int HoursPerWeek,
        int PaymentMethodId,
        DateTime StartedDate,
        DateTime EndedDate
    );
    
}
