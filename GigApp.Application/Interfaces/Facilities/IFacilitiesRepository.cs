using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Facilities
{
    public interface IFacilitiesRepository
    {
        Task<BaseResult> AddFacility(Client client, string auth0Id);
        Task<BaseResult> EditFacility(Client client, string auth0Id);
        Task<BaseResult> GetFacilities(string auth0Id, bool isAdmin);
        Task<BaseResult> AddClientLocation(ClientLocation clientLocation, string auth0Id);
        Task<BaseResult> GetFacility(long id);
        Task<BaseResult> DeleteFacilities(List<long> ids);
        Task<BaseResult> GetFacilities();
        Task<BaseResult> GetClientLocationsAsync(long clientId);
        Task<BaseResult> GetJobManagers(long clientId);
        Task<BaseResult> AddJobManager(long clientId, string email);
        Task<BaseResult> DeleteJobManager(long clientId, long userId);

    }
}
