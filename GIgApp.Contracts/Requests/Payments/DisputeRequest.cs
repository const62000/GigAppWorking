using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Payments
{
    public record DisputeRequest(string Type,string Reason,string Description,string Attachment, int HiredJobId ,long TimeSheetId);
}
