using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Events.Queries.GetAllEvents;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Services;

public class GetAllEventsService(IAppDbContext context)
{
    public async Task<List<AllEventsDto>> GetAllEvents(DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        var eventsInDb = await context.Events.Include(e => e.EventExceptions).ToListAsync(cancellationToken);
        
        var outputInstances = new List<AllEventsDto>();

        foreach (var e in eventsInDb)
        {
            var exceptionsByDate = e.EventExceptions
                .ToDictionary(ex => ex.OriginalOccurrence, ex => ex);
            
            if (e.StartDate is null && e.EndDate is null)
                continue;


            if (!e.IsRecurring || string.IsNullOrEmpty(e.RRule))
            {
                if (e.StartDate < to && e.EndDate > from)
                {
                    if (exceptionsByDate.TryGetValue(e.StartDate.Value, out var exception))
                    {
                        if (exception.IsCancelled)
                            continue;
                    }
                    outputInstances.Add(new AllEventsDto
                    {
                        Id = e.Id,
                        Title = e.Title,
                        StartDate = e.StartDate!.Value,
                        EndDate = e.EndDate!.Value
                    });
                    continue;
                }
            }

            var calendarEvent = new CalendarEvent
            {
                DtStart = new CalDateTime(e.StartDate.Value),
                DtEnd = new CalDateTime(e.EndDate.Value),
                RecurrenceRules = new List<RecurrencePattern>
                {
                    new RecurrencePattern(e.RRule)
                }
            };
            
            var calendar = new Calendar();
            calendar.Events.Add(calendarEvent);

            var occurrences = calendar.GetOccurrences(new CalDateTime(from))
                .TakeWhileBefore(new CalDateTime(to));
            
            foreach (var occurrence in occurrences)
            {
                var start = occurrence.Period.StartTime.Value;
                
                if (exceptionsByDate.TryGetValue(start, out var exception))
                {
                    if (exception.IsCancelled)
                    {
                        continue;
                    }
                }
                
                outputInstances.Add(new AllEventsDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDate = occurrence.Period.StartTime.Value,
                    EndDate = occurrence.Period.StartTime.Value.AddMinutes(e.DurationInMinutes)
                });
            }
        }

        return outputInstances;
    }
}