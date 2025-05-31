using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Options.MailServer
{
    public class MailServerOption
    {
        private string sectionName = "EmailSettings";
        public string SectionName => sectionName;
        public string SmtpServer {  get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;

    }
}
