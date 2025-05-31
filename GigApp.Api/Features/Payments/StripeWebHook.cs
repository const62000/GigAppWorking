using Carter;
using GIgApp.Contracts.Requests.Payments;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using Mapster;
using MediatR;
using System.IO;
using System.Threading.Tasks;
using Stripe;
using static GigApp.Application.CQRS.Implementations.Payments.Queries.StripeWebHooks;

namespace GigApp.Api.Features.Payments;

public class StripeWebHook : ICarterModule
{
    private readonly string _endpointSecret = "whsec_prdznntcy4y0Ap8DZNgFAvsKKlmiUj2Q"; // From Stripe Dashboard

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EndPoints.StripeWebHook, async (HttpContext ctx, ISender sender) =>
        {
            // 1) Read the request body as text
            using var reader = new StreamReader(ctx.Request.Body);
            var json = await reader.ReadToEndAsync();

            // 2) Grab the signature from Stripe
            var signatureHeader = ctx.Request.Headers["Stripe-Signature"];

            Event stripeEvent;
            try
            {
                // 3) Verify the event's signature
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    _endpointSecret,
                    throwOnApiVersionMismatch: false
                );
            }
            catch (StripeException e)
            {
                return Results.BadRequest(e.Message);
            }

            // 4) Handle the event
            var result = await sender.Send(new Query { Event = stripeEvent });
            // 5) Return 200 to acknowledge receipt of the event
            return Results.Ok(result);
        });
    }
}