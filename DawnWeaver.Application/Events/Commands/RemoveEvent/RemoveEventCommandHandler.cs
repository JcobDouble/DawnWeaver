using DawnWeaver.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Commands.RemoveEvent;

public class RemoveEventCommandHandler(IAppDbContext context) : IRequestHandler<RemoveEventCommand>
{
    public async Task Handle(RemoveEventCommand request, CancellationToken cancellationToken)
    {
        var eventInDb = await context.Events.FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);
        
        if (eventInDb == null)
        {
            throw new KeyNotFoundException($"Event with ID {request.EventId} not found.");
        }
        
        context.Events.Remove(eventInDb);
        await context.SaveChangesAsync(cancellationToken);
    }
}