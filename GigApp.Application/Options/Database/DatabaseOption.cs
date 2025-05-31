using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Options.Database
{
    public class DatabaseOption
    {
        private string sectionName = "Database";
        public string SectionName => sectionName;
        public string Type { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }
}
