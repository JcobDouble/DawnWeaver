using DawnWeaver.Application.Events.Queries.GetAllEvents;
using DawnWeaver.Application.Services;
using DawnWeaver.Domain.Common;
using DawnWeaver.Domain.Enums;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using MediatR;

namespace DawnWeaver.Application.Events.Queries.CheckForCollisions;

public class CheckForCollisionsQueryHandler(GetAllEventsService getAllEventsService) : IRequestHandler<CheckForCollisionsQuery, ResultT<AllEventsViewModel>>
{
    public async Task<ResultT<AllEventsViewModel>> Handle(CheckForCollisionsQuery request, CancellationToken cancellationToken)
    {
        var conflictingEvents = new Dictionary<Guid, AllEventsDto>();

        if (request.RecurrenceType == RecurrenceType.AllOccurrences)
        {
            var yearForwardEvents = await getAllEventsService.GetAllEvents(request.StartDate, request.StartDate.AddYears(1), cancellationToken);
            var eventsExcludingCurrent = yearForwardEvents.Where(e => e.Id != request.Id).ToList();

            var calendarEvent = new CalendarEvent
            {
                DtStart = new CalDateTime(request.StartDate),
                DtEnd = new CalDateTime(request.StartDate.AddMinutes(request.DurationInMinutes)),
                RecurrenceRules = new List<RecurrencePattern>
                {
                    new RecurrencePattern(request.RRule)
                }
            };
            
            var yearForwardOccurrences = calendarEvent.GetOccurrences(new CalDateTime(request.StartDate))
                .TakeWhileBefore(new CalDateTime(request.StartDate.AddYears(1)));
            
            foreach (var occurrence in yearForwardOccurrences)
            {
                foreach (var events in eventsExcludingCurrent)
                {
                    if (occurrence.Period.StartTime.Value < events.EndDate && events.StartDate < occurrence.Period.EndTime!.Value)
                    {
                        conflictingEvents.TryAdd(events.Id, events);
                    }
                }
            }
        }

        if (request.RecurrenceType == RecurrenceType.OnlyThis)
        {
            var occurrencesWithinEdited = await getAllEventsService.GetAllEvents(request.StartDate, request.StartDate.AddMinutes(request.DurationInMinutes), cancellationToken);
            
            foreach (var events in occurrencesWithinEdited.Where(e => e.Id != request.Id && e.StartDate != request.OriginalStartDate))
            {
                conflictingEvents.TryAdd(events.Id, events);
            }
        }
        
        if (request.RecurrenceType == RecurrenceType.ThisAndFollowing)
        {
            // to impelement
        }
        
        var result = new AllEventsViewModel
        {
            Events = conflictingEvents.Values.ToList()
        };

        return Result.Success(result);
        // jeśli zwrócona lista jest pusta to nie ma kolizji
    }
}