using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Vendors
{
    public interface IVendorRepository
    {
        Task<BaseResult> AddVendor(Vendor vendor, string auth0Id);
        Task<BaseResult> DeleteVendors(List<int> ids);
        Task<BaseResult> GetVendors(string auth0Id, bool isAdmin = false);
        Task<BaseResult> GetVendorById(int vendorId, string auth0Id, bool isAdmin = false);
        Task<BaseResult> AddStaff(string auth0Id, string email);
        Task<BaseResult> AddStaff(int vendorId, string email);
        Task<BaseResult> GetStaff(string auth0Id, bool isAdmin = false);
        Task<BaseResult> UpdateVendor(Vendor vendor);
        Task<BaseResult> AddVendorManager(int vendorId, string email);
    }
}
