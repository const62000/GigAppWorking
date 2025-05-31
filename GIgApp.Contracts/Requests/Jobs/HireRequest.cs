using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Jobs
{
    public record HireRequest(long Id,long JobId, long FreelancerId, string Status, string? Note, DateTime StartTime, DateTime EndTime);
}
