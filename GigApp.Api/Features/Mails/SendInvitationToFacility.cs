using Carter;
using GIgApp.Contracts.Requests.Facilities;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using MediatR;
using static GigApp.Application.CQRS.Implementations.Mails.Queries.SendInvitationToFacility;

namespace GigApp.Api.Features.Mails
{
    public class SendInvitationToFacility : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Facility_Invitation, async (FacilityInvitation request, ISender sender) =>
            {
                var result = await sender.Send(new Query { Email = request.Email });
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(new BaseFailResult(result.Data, result.Status, result.Message));
            });
        }
    }
}
