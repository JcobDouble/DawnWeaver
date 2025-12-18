using DawnWeaver.Application.Events.Commands.AddEvent;
using Ical.Net.DataTypes;

namespace DawnWeaver.Application;

public static class RRuleBuilder
{
    public static string BuildRRule(RecurrenceDto recurrence)
    {
        var pattern = new RecurrencePattern(recurrence.Frequency)
        {
            Interval = recurrence.Interval,
        };
        
        if (recurrence.ByDays != null && recurrence.ByDays.Any())
        {
            pattern.ByDay = recurrence.ByDays
                .Select(d => new WeekDay(d))
                .ToList();
        }

        if (recurrence.Until != null)
        {
            pattern.Until = new CalDateTime(recurrence.Until.Value);
        }
        
        return pattern.ToString()!;
    }
}