using GIgApp.Contracts.Requests.Vendor;
using GIgApp.Contracts.Responses;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Requests.BankAccount;

namespace VMS.Client.Repositories.Abstractions;

public interface IVendorRepository
{
    Task<BaseResult> AddVendor(VendorRequest request);
    Task<BaseResult> AddStaff(string email, int vendorId = 0);
    Task<BaseResult> AddBankAccount(BankAccountRequet request);
    Task<List<User>> GetStaff();
    Task<BaseResult> DeleteStaff(long userId);
    Task<List<Vendor>> GetVendors();
    Task<Vendor> GetVendorById(int vendorId);
    Task<BaseResult> UpdateVendor(VendorRequest request, int vendorId);
    Task<BaseResult> AddVendorManager(AssignVendorManagerRequest request);
    Task<BaseResult> DeleteVendors(DeleteVendorsRequest request);
}