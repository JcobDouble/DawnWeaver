using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Common.Mapping;
using DawnWeaver.Application.Events.Queries.GetEventDetail;
using DawnWeaver.Domain.Entities;
using DawnWeaver.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler(IAppDbContext context) :IRequestHandler<UpdateEventCommand, EventDetailViewModel>
{
    public async Task<EventDetailViewModel> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventInDb = await context.Events.Include(e => e.EventType).FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (eventInDb == null)
        {
            throw new Exception("Event not found");
        }
        
        var duration = request.DurationInMinutes ?? eventInDb.DurationInMinutes;
        var endDate = request.StartDate is not null ? request.StartDate?.AddMinutes(duration) : eventInDb.EndDate;


        if (request.RecurrenceType == RecurrenceType.AllOccurrences)
        {
            eventInDb.Title = request.Title ?? eventInDb.Title;
            eventInDb.Description = request.Description;
            eventInDb.StartDate = request.StartDate ?? eventInDb.StartDate;
            eventInDb.EndDate = endDate;
            eventInDb.IsAllDay = request.IsAllDay ?? eventInDb.IsAllDay;
            eventInDb.IsRecurring = request.IsRecurring ?? eventInDb.IsRecurring;
            eventInDb.DurationInMinutes = request.DurationInMinutes ?? eventInDb.DurationInMinutes;
            eventInDb.EventTypeId = request.EventTypeId ?? eventInDb.EventTypeId;
            eventInDb.Status = EventStatus.Confirmed;
            if(request.IsRecurring is null)
                eventInDb.RRule = eventInDb.RRule;
            else if(request.IsRecurring == true)
                eventInDb.RRule = request.RecurrenceDto is not null ? RRuleBuilder.BuildRRule(request.RecurrenceDto) : eventInDb.RRule;
            else
                eventInDb.RRule = null;
        }

        if (request.RecurrenceType == RecurrenceType.OnlyThis)
        {
            var eventException = new EventException
            {
                OriginalOccurrence = request.OccurrenceStart.Value,
                IsCancelled = true,
                EventId = request.Id
            };
            
            context.EventExceptions.Add(eventException);
            
            var newEvent = new Event
            {
                Title = request.Title ?? eventInDb.Title,
                Description = request.Description,
                StartDate = request.StartDate ?? request.OccurrenceStart,
                EndDate = endDate,
                IsAllDay = request.IsAllDay ?? eventInDb.IsAllDay,
                IsRecurring = false,
                DurationInMinutes = request.DurationInMinutes ?? eventInDb.DurationInMinutes,
                EventTypeId = request.EventTypeId ?? eventInDb.EventTypeId,
                RRule = null,
                Status = EventStatus.Confirmed
            };
            
            context.Events.Add(newEvent);
            
            // Co jeśli użytkownik kliknie na główny event
        }

        // Dodać DbSet<EventExceptions> i DI potrzebne
        
        await context.SaveChangesAsync(cancellationToken);
        
        return eventInDb.MapToEventDetailViewModel();
    }
}