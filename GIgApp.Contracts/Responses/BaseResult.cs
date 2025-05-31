using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses
{
    public record BaseResult(object Data, bool Status, string Message);
    public record BaseFailResult(object Data, bool Status, string Error);
}
