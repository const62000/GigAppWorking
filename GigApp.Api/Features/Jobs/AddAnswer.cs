using Carter;
using GIgApp.Contracts.Requests.JobQuestion;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.Collections.Generic;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Jobs.Commands.AddAnswer;

namespace GigApp.Api.Features.Jobs
{
    public class AddAnswer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Answer_EndPoint, async (List<AnswerRequest> request,HttpContext ctx, ISender sender) =>
            {
                var userClaims = ctx.User;
                string Auth0Id = userClaims.FindFirstValue(ClaimTypes.NameIdentifier)?.Replace("auth0|", "")??string.Empty;
                var results = new List<BaseResult>();
                var commands = request.Adapt<List<Command>>();
                foreach (var command in commands)
                {
                    command.Auth0Id = Auth0Id;
                    results.Add(await sender.Send(command));
                }
                var failAnswers = results.Where(x => !x.Status).Select(x => x.Data).ToList();
                if(failAnswers.Count ==0)
                    return Results.Ok(results.FirstOrDefault());
                return Results.BadRequest(new BaseFailResult(failAnswers,false,string.Empty));
            });
        }
    }
}
