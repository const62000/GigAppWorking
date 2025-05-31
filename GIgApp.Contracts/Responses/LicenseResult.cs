using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses
{
    public class LicenseResult
    {
        public string LicenceName { get; set; } = string.Empty;
        public DateTime DateOfIssue { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public string IssueBy { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
    }
}
