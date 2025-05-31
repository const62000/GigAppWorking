using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Payments
{
    public record ProcessDisputeRequest(long DisputeId, string ActionMessage, string ActionStatus, string Attachment);
}
