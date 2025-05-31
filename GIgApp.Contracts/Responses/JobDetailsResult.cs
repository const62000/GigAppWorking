using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses
{
    public record JobDetailsResult(long Id, string Title, string Description, string? Requirements, DateOnly Date, TimeOnly Time, decimal Rate, string Status);
}
