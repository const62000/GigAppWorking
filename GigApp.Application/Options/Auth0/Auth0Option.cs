using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Options.Auth0
{
    public class Auth0Option
    {
        private string sectionName = "Auth0";
        public string SectionName => sectionName;
        public string Domain { get; set; } = string.Empty;
        public string DomainName { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Connection { get; set; } = string.Empty;
        public string ManagementApiToken { get; set; } = string.Empty;
    }
}
