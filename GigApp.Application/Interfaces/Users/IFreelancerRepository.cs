using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Users
{
    public interface IFreelancerRepository
    {
        Task<BaseResult> AddFreelancer(long userId);
        Task<IEnumerable<FreelancerLicense>> GetFreelancerLicenseInfoAsync(long userId);
        Task<BaseResult> GetFreelancers();
        Task<BaseResult> DisableFreelancer(long userId);
        Task<BaseResult> EnableFreelancer(long userId);
    }
}
