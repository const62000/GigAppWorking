using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.CQRS.Implementations.Jobs.Commands;
using GigApp.Application.Interfaces.Facilities;
using GigApp.Application.Interfaces.Users;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.LicenseInfo.Queries
{
    public class GetFreelancerLicenseInfo
    {
        public class Query : IQuery
        {
            public long FreelancerUserId { get; set; }  
        }
        public class Handler(IFreelancerRepository _freelancerRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var licenseInfo = await _freelancerRepository.GetFreelancerLicenseInfoAsync(request.FreelancerUserId);
                    if (licenseInfo == null)
                    {
                        return new BaseResult(new { }, false, $"License Information with ID {request.FreelancerUserId} not found.");
                    }
                    return new BaseResult(licenseInfo, true, "Freelancers License details fetched successfully!");
                }
                catch (Exception ex)
                {
                    return new BaseResult(new {ex.Message}, false, $"An error occurred while fetching License details: {ex.Message}");
                }
            }
        }
    }
}
