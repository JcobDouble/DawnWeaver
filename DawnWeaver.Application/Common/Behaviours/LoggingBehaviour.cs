using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace DawnWeaver.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger) : IRequestPreProcessor<TRequest>
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("App Request {Name} {@Request}", requestName, request);
        
        return Task.CompletedTask;
    }
}