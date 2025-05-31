using GigApp.Application.Interfaces.Addresses;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Infrastructure.Repositories.Addresses
{
    internal class AddressRepository(ApplicationDbContext _applicationDbContext) : IAddressRepository
    {
        public async Task<BaseResult> GetAllCountries()
        {
            var countries = await _applicationDbContext.Addresses.Select(x => x.Country).Distinct().ToListAsync();
            return new BaseResult(countries, true, string.Empty);
        }

        public async Task<BaseResult> GetCities(string country)
        {
            var cities = await _applicationDbContext.Addresses.Where(x=>x.Country == country).Select(x => x.City).Distinct().ToListAsync();
            return new BaseResult(cities, true, string.Empty);
        }
    }
}
