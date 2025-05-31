using GIgApp.Contracts.Responses;
using MediatR;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class LoggingBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Log the request
        Debug.WriteLine($"Handling {typeof(TRequest).FullName}");
        
        // Call the next handler in the pipeline
        var response = await next(); // Ensure this is called with the correct parameters
        
        // Log the response
        Debug.WriteLine($"Handled {typeof(TResponse).FullName}");
        


        return response;
    }
}