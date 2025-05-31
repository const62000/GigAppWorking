using GigApp.Application.Interfaces.Users;
using GigApp.Application.Interfaces.Vendors;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Infrastructure.Repositories.Vendors
{
    public class VendorRepository(ApplicationDbContext _applicationDbContext, IUserRepository _userRepository) : IVendorRepository
    {
        public async Task<BaseResult> AddVendor(Vendor vendor, string auth0Id)
        {
            if (!string.IsNullOrEmpty(auth0Id))
                vendor.UserId = await _userRepository.GetUserId(auth0Id);
            await _applicationDbContext.Vendors.AddAsync(vendor);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { id = vendor.Id }, true, Messages.Vendor_Created);
        }
        public async Task<BaseResult> UpdateVendor(Vendor vendor)
        {
            await _applicationDbContext.Vendors.Where(x => x.Id == vendor.Id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Name, vendor.Name)
            .SetProperty(x => x.ServicesOffered, vendor.ServicesOffered)
            .SetProperty(x => x.Certifications, vendor.Certifications)
            .SetProperty(x => x.Status, vendor.Status));
            return new BaseResult(new { }, true, Messages.Vendor_Updated);
        }

        public async Task<BaseResult> DeleteVendors(List<int> ids)
        {
            var result = await _applicationDbContext.Vendors.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
            if (result > 0)
                return new BaseResult(new { }, true, Messages.Vendor_Deleted);
            return new BaseResult(new { }, true, Messages.Vendor_Deleted_Fail);
        }

        public async Task<BaseResult> GetVendors(string auth0Id, bool isAdmin = false)
        {
            var vendors = isAdmin ? await _applicationDbContext.Vendors.IncludeVendorDetails().ToListAsync() : await _applicationDbContext.Vendors.IncludeVendorDetails().Where(x => x.User!.Auth0UserId == auth0Id).IncludeVendorDetails().ToListAsync();
            return new BaseResult(vendors, true, string.Empty);
        }
        public async Task<BaseResult> AddStaff(string auth0Id, string email)
        {
            var vendor = await _applicationDbContext.Vendors.FirstOrDefaultAsync(x => x.User.Auth0UserId == auth0Id);
            var user = await _applicationDbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (vendor == null)
                return new BaseResult(new { }, false, Messages.Vendor_Not_Found);
            var userVendor = new UserVendor
            {
                VendorId = vendor.Id,
                UserId = user.Id
            };
            await _applicationDbContext.UserVendors.AddAsync(userVendor);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, Messages.Staff_Added);
        }
        public async Task<BaseResult> GetVendorById(int vendorId, string auth0Id, bool isAdmin = false)
        {
            var vendor = isAdmin ? await _applicationDbContext.Vendors.IncludeVendorDetails().FirstOrDefaultAsync(x => x.Id == vendorId) : await _applicationDbContext.Vendors.IncludeVendorDetails().FirstOrDefaultAsync(x => x.Id == vendorId && x.User!.Auth0UserId == auth0Id);
            return new BaseResult(vendor!, true, string.Empty);
        }
        public async Task<BaseResult> AddStaff(int vendorId, string email)
        {
            var vendor = await _applicationDbContext.Vendors.FirstOrDefaultAsync(x => x.Id == vendorId);
            if (vendor == null)
                return new BaseResult(new { }, false, Messages.Vendor_Not_Found);
            var user = await _applicationDbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null)
                return new BaseResult(new { }, false, Messages.User_Not_Found);
            var userVendor = new UserVendor
            {
                VendorId = vendorId,
                UserId = user.Id
            };
            await _applicationDbContext.UserVendors.AddAsync(userVendor);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new { }, true, Messages.Staff_Added);
        }
        public async Task<BaseResult> GetStaff(string auth0Id, bool isAdmin = false)
        {
            var staff = isAdmin ? await _applicationDbContext.UserVendors.IncludeUserVendorDetails().ToListAsync() : await _applicationDbContext.UserVendors.Where(x => x.Vendor!.User!.Auth0UserId == auth0Id).IncludeUserVendorDetails().ToListAsync();
            return new BaseResult(staff, true, string.Empty);
        }
        public async Task<BaseResult> AddVendorManager(int vendorId, string email)
        {
            var user = await _applicationDbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null)
                return new BaseResult(new { }, false, Messages.User_Not_Found);
            await _applicationDbContext.Vendors.Where(x => x.Id == vendorId).ExecuteUpdateAsync(x => x.SetProperty(x => x.UserId, user.Id));
            return new BaseResult(new { }, true, Messages.Vendor_Manager_Added);
        }

    }
}
