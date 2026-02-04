namespace DawnWeaver.Application.Events.Queries.GetAllEvents;

public class AllEventsDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool HasConflict { get; set; }
}