using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Vendor
{
    public record DeleteVendorsRequest(List<int> VendorIds);
}
