using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Common.Mapping;
using DawnWeaver.Application.Events.Queries.GetEventDetail;
using DawnWeaver.Domain.Entities;
using DawnWeaver.Domain.Enums;
using Ical.Net.DataTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler(IAppDbContext context) :IRequestHandler<UpdateEventCommand, EventDetailViewModel>
{
    public async Task<EventDetailViewModel> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventInDb = await context.Events.Include(e => e.EventType).FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        Event resultEvent = null!;
        
        
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
            eventInDb.EventType = context.EventTypes.FirstOrDefault(e => e.Id == eventInDb.EventTypeId)!;
            eventInDb.Status = EventStatus.Confirmed;
            if(request.IsRecurring is null)
                eventInDb.RRule = eventInDb.RRule;
            else if(request.IsRecurring == true)
                eventInDb.RRule = request.RecurrenceDto is not null ? RRuleBuilder.BuildRRule(request.RecurrenceDto) : eventInDb.RRule;
            else
                eventInDb.RRule = null;

            resultEvent = eventInDb;
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
            
            resultEvent = new Event
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

            resultEvent.EventType = context.EventTypes.FirstOrDefault(e => e.Id == resultEvent.EventTypeId)!;
            
            context.Events.Add(resultEvent);
            
            // Co jeśli użytkownik kliknie na główny event
        }
        
        if (request.RecurrenceType == RecurrenceType.ThisAndFollowing)
        {
            var rule = new RecurrencePattern(eventInDb.RRule);
            rule.Until = new CalDateTime(request.StartDate.Value.Subtract(TimeSpan.FromMinutes(1)));
            rule.Count = null;
            
            resultEvent = new Event
            {
                Title = request.Title ?? eventInDb.Title,
                Description = request.Description,
                StartDate = request.StartDate ?? eventInDb.StartDate,
                EndDate = endDate,
                IsAllDay = request.IsAllDay ?? eventInDb.IsAllDay,
                IsRecurring = true,
                DurationInMinutes = duration,
                EventTypeId = request.EventTypeId ?? eventInDb.EventTypeId,
                RRule = request.RecurrenceDto is not null ? RRuleBuilder.BuildRRule(request.RecurrenceDto) : eventInDb.RRule,
                Status = EventStatus.Confirmed
            };
            
            resultEvent.EventType = context.EventTypes.FirstOrDefault(e => e.Id == resultEvent.EventTypeId)!;
            
            eventInDb.RRule = rule.ToString();
            
            var eventExceptions = eventInDb.EventExceptions;

            if (eventInDb.EventExceptions.Any())
            {
                foreach (var eventException in eventExceptions)
                {
                    if (eventException.OriginalOccurrence >= request.StartDate)
                    {
                        context.EventExceptions.Remove(eventException);
                    }
                }
            }
            
            context.Events.Add(resultEvent);
        }
        
        await context.SaveChangesAsync(cancellationToken);

        return resultEvent.MapToEventDetailViewModel();
    }
}