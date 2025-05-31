using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Jobs
{
    public class AnswerInput
    {
        public long JobQuestionId { get; set; }
        public string Answer { get; set; } = string.Empty;

    }
}
