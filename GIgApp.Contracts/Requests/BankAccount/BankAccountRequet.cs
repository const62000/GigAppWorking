using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.BankAccount
{
    public class BankAccountRequet
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountType { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string BankSwiftCode { get; set; }
        public string BankCountry { get; set; }
        public string Status { get; set; }
        public string BankToken { get; set; }

        public BankAccountRequet(string bankName, string bankAccountType, string bankAccountNumber, string bankAccountName, string bankSwiftCode, string bankCountry, string status, string bankToken)
        {
            BankName = bankName;
            BankAccountType = bankAccountType;
            BankAccountNumber = bankAccountNumber;
            BankAccountName = bankAccountName;
            BankSwiftCode = bankSwiftCode;
            BankCountry = bankCountry;
            Status = status;
            BankToken = bankToken;
        }
    }
}
