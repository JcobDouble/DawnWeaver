using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DawnWeaver.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(ILogger<TRequest> logger, Stopwatch timer) : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        timer.Start();
        
        var response = await next();
        
        timer.Stop();
        
        var elapsedMilliseconds = timer.ElapsedMilliseconds;
        
        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogInformation("App Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", requestName, elapsedMilliseconds, request);
        }
        
        return response;
    }
}