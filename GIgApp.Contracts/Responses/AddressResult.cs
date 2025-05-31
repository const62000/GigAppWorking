using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses
{
    public class AddressResult
    {
        public string StreetAddress1 { get; set; } = string.Empty;
        public string StreetAddress2 { get; set; } = String.Empty;
        public string City { get; set; } = string.Empty;
        public string StateProvince { get; set; } = string.Empty ;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
