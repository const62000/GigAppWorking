using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses
{
    public class CurrentUserResult
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePic { get; set; } = string.Empty;
        public List<string> UserType { get; set; } = new List<string>();
        public AddressResult Address { get; set; } = new();
        public List<LicenseResult> Licenses { get; set; } = new List<LicenseResult>();
    }
}
