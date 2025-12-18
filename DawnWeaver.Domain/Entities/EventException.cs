namespace DawnWeaver.Domain.Entities;

public class EventException
{
    public Guid Id { get; set; }
    public DateTime OriginalOccurrence { get; set; }
    public bool IsCancelled { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;
}