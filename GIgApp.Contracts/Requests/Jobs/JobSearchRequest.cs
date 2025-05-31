using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Jobs
{
    public record JobSearchRequest(int Page, int PerPage, string SearchQuery, Dictionary<string, string> Filters, string SortBy, bool Ascending,decimal Latitude,decimal Longitude,decimal Miles);
}
