using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.TimeSheet
{
    public record ClockInOutRequest( int HiringId,string Note,Location Location);
    public record Location(decimal Latitude, decimal Longitude);
   
}
