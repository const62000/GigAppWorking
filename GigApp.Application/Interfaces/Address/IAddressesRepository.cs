using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Addresses
{ 
    public interface IAddressesRepository
    {
        Task<BaseResult> GetAddressAsync(long userId);
        Task<BaseResult> AddAddressAsync(string auth0Id, Address address);
    }
}
