using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Users
{
    public interface IFacilityManagerRepository
    {
        Task<BaseResult> AddFacilityManager(long userId);
    }
}
