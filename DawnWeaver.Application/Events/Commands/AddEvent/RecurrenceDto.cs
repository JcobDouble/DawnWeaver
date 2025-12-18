namespace DawnWeaver.Application.Events.Commands.AddEvent;

public class RecurrenceDto
{    
    public string Frequency { get; set; } = "DAILY";
    public int Interval { get; set; } = 1;
    public List<DayOfWeek>? ByDays { get; set; }
    public DateTime? Until { get; set; }
}