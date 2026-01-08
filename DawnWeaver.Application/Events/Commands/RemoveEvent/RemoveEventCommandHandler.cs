using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Domain.Entities;
using DawnWeaver.Domain.Enums;
using Ical.Net.DataTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Commands.RemoveEvent;

public class RemoveEventCommandHandler(IAppDbContext context) : IRequestHandler<RemoveEventCommand>
{
    public async Task Handle(RemoveEventCommand request, CancellationToken cancellationToken)
    {
        var eventInDb = await context.Events.Include(e => e.EventExceptions).FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);
        
        if (eventInDb == null)
        {
            throw new KeyNotFoundException($"Event with ID {request.EventId} not found.");
        }
        
        if(!eventInDb.IsRecurring || request.RecurrenceType == RecurrenceType.AllOccurrences)
        {
            context.Events.Remove(eventInDb);
        }

        else if(eventInDb.IsRecurring)
        {
            if(request.RecurrenceType == RecurrenceType.OnlyThis)
            {
                var eventException = new EventException
                {
                    OriginalOccurrence = request.OccurrenceDate,
                    IsCancelled = true,
                    EventId = eventInDb.Id
                };
                
                context.EventExceptions.Add(eventException);
            }
            
            else if(request.RecurrenceType == RecurrenceType.ThisAndFollowing)
            {
                var rule = new RecurrencePattern(eventInDb.RRule);
                rule.Until = new CalDateTime(request.OccurrenceDate.Subtract(TimeSpan.FromMinutes(1)));
                rule.Count = null;
                
                eventInDb.RRule = rule.ToString();

                var eventExceptions = eventInDb.EventExceptions;

                if (eventExceptions.Any())
                {
                    // zrobić to samo w update handler zamiast foreach

                    var exceptionsToRemove = eventInDb.EventExceptions
                        .Where(e => e.OriginalOccurrence >= request.OccurrenceDate)
                        .ToList();
                    
                    context.EventExceptions.RemoveRange(exceptionsToRemove);
                }
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }
}