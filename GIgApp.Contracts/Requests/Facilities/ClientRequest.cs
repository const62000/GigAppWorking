using GIgApp.Contracts.Requests.Signup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Facilities
{
    public class ClientRequest(string Name, AddressRequest Address)
    {
        public string Name { get; set; } = Name;
        public AddressRequest Address { get; set; } = Address;
    }
}
