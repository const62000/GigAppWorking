using Carter;
using GIgApp.Contracts.Shared;
using MediatR;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using static GigApp.Application.CQRS.Implementations.Files.Commands.AddFile;

namespace GigApp.Api.Features.Files.Commands
{
    public class AddFile : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(EndPoints.Upload_File_EndPoint, async (HttpContext ctx, ISender sender) =>
            {
                var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}{ctx.Request.PathBase}";
                var fileName =  ctx.Request.Form["fileName"];
                StringValues text = "License";
                var file = ctx.Request.Form.Files.Count() > 0 ? ctx.Request.Form.Files.First() : null;
                var command = new Command
                {
                    FileName = fileName.ToString()?? file!.FileName??"" ,
                    File = file,
                    Text = text.ToString(),
                    BaseUrl = baseUrl
                };
                var result = await sender.Send(command);
                if (result.Status)
                    return Results.Ok(result);
                else
                    return Results.BadRequest(result);
            });
        }
    }
}
