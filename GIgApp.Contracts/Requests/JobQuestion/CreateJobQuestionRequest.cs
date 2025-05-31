using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.JobQuestion
{
    public record CreateJobQuestionRequest(long JobId, string Title, DateTime CurrentTimeStamp); 
}
