using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses.Jobs
{
    public record JobStatusResult(long Id, string Title, string Status);
}
