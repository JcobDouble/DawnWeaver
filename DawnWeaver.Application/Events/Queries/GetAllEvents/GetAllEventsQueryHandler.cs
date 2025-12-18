using DawnWeaver.Application.Common.Interfaces;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler(IAppDbContext context) : IRequestHandler<GetAllEventsQuery, AllEventsViewModel>
{
    public async Task<AllEventsViewModel> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var eventsInDb = await context.Events.ToListAsync(cancellationToken);
        
        var outputInstances = new List<AllEventsDto>();

        foreach (var e in eventsInDb)
        {
            if (e.StartDate is null && e.EndDate is null)
                continue;


            if (!e.IsRecurring || string.IsNullOrEmpty(e.RRule))
            {
                if (e.StartDate < request.To && e.EndDate > request.From)
                {
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

            var occurrences = calendar.GetOccurrences(new CalDateTime(request.From))
                .TakeWhileBefore(new CalDateTime(request.To));

            var exceptionsByDate = e.EventExceptions
                .ToDictionary(ex => ex.OriginalOccurrence, ex => ex);
            
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
                    EndDate = occurrence.Period.EndTime.Value
                });
            }
        }

        return new AllEventsViewModel
        {
            Events = outputInstances
        };
    }
}