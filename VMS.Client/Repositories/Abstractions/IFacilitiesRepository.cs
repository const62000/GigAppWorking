using GigApp.Domain.Entities;
using GIgApp.Contracts.Shared;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Requests.Vendor;

namespace VMS.Client.Repositories.Abstractions
{
    public interface IFacilitiesRepository
    {
        Task<List<GigApp.Domain.Entities.Client>> GetClientsAsync();
        Task<GigApp.Domain.Entities.Client> GetClientAsync(long id);
        Task<BaseResult> AddClientAsync(ClientRequest clientRequest);
        Task<BaseResult> AddClientLocationAsync(ClientLocationRequest clientLocationRequest);
        Task<bool> DeleteClientsAsync(DeleteClientRequest request);
        Task<BaseResult> EditFacility(ClientRequest clientRequest, long id);
        Task<List<ClientLocation>> GetClientLocationsAsync(long clientId);
        Task<List<User>> GetJobManagers(long clientId);
        Task<BaseResult> AddJobManager(AssignJobManagerRequest request);
        Task<BaseResult> DeleteJobManager(long clientId, long userId);
    }
}
