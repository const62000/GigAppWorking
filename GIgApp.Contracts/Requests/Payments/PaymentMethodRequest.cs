using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Requests.Payments
{
    public record PaymentMethodRequest(string CardLastFour,string CardBrand, int ExpMonth,int ExpYear ,
        string CardholderName ,string StripePaymentMethodId ,string StripeCardId ,bool? IsDefault );

}
