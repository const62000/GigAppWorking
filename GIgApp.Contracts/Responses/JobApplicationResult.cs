using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses
{
    public record JobApplicationResult(long Id, long? FreelancerUserId, string Status);
}
