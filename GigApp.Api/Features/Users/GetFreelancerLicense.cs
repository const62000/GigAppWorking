using Carter;
using GIgApp.Contracts.Shared;
using MediatR;
using GigApp.Application.CQRS.Implementations.LicenseInfo.Queries;
using GIgApp.Contracts.Responses;

namespace GigApp.Api.Features.Users
{
    public class GetFreelancerLicense : ICarterModule 
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(EndPoints.GetFreelancerLicenseInfo, async (long freelancerUserId, ISender send) =>
            {
                 var query = new GetFreelancerLicenseInfo.Query{ FreelancerUserId= freelancerUserId};
                 var result = await send.Send(query);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });
        }
    }
}
