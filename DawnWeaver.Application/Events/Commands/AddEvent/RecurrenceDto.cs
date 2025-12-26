using Ical.Net;

namespace DawnWeaver.Application.Events.Commands.AddEvent;

public class RecurrenceDto
{    
    public FrequencyType Frequency { get; set; }
    public int Interval { get; set; } = 1;
    public List<DayOfWeek>? ByDays { get; set; }
    public DateTime? Until { get; set; }
}