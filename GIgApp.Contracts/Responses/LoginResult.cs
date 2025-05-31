using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses
{
    public record LoginResult(string access_token, int expires_in,string token_type);
    public record LoginFailResult(string error,string error_description);
}
