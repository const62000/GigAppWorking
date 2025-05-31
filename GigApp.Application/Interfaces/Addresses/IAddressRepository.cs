using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Addresses
{
    public interface IAddressRepository
    {
        Task<BaseResult> GetAllCountries();
        Task<BaseResult> GetCities(string country);
    }
}
