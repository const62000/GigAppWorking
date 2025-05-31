using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Request { get; set; } = string.Empty;
        public string? Response { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? EntityId { get; set; }
    }
}
