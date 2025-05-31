using MediatR;
using GIgApp.Contracts.Responses;
using GigApp.Application.Interfaces.Users;

namespace GigApp.Application.CQRS.Implementations.Users.Queries;

public class ChangeFreelancerStatus
{
    public class Query : IRequest<BaseResult>
    {
        public long UserId { get; set; }
        public bool Disabled { get; set; }
    }
    public class Handler : IRequestHandler<Query, BaseResult>
    {
        private readonly IFreelancerRepository _freelancerRepository;
        public Handler(IFreelancerRepository freelancerRepository)
        {
            _freelancerRepository = freelancerRepository;
        }
        public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request.Disabled)
            {
                return await _freelancerRepository.DisableFreelancer(request.UserId);
            }
            else
            {
                return await _freelancerRepository.EnableFreelancer(request.UserId);
            }
        }
    }
}

