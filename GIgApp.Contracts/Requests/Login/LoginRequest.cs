using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Login
{
    public record LoginRequest(string Email,string Password,string firebase_token,string device_info);
}
