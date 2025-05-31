using GigApp.Application.Interfaces.Users;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Responses;


namespace GigApp.Infrastructure.Repositories.Users
{
    internal class FacilityManagerRepository(ApplicationDbContext _applicationDbContext) : IFacilityManagerRepository
    {
        public async Task<BaseResult> AddFacilityManager(long userId)
        {
            var  facilityManager= new FacilityManager
            {
                UserId = userId,
            };
            await _applicationDbContext.FacilityManagers.AddAsync(facilityManager);
            await _applicationDbContext.SaveChangesAsync();
            return new BaseResult(new {},true,string.Empty);
        }
    }
}
