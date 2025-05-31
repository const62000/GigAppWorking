using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses
{
    public record SignupResult(string _id,string email,string username,bool email_verified);
    public record SignupFailResult(string name, string code,string message);
    public record DataIdentifier(string identifierType);
}
