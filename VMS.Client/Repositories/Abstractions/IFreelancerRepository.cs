
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;

namespace VMS.Client.Repositories.Abstractions;

public interface IFreelancerRepository
{
    Task<List<User>> GetFreelancers();
    Task<BaseResult> ChangeFreelancerStatus(long userId, bool disabled);
}

