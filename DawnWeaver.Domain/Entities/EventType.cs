namespace DawnWeaver.Domain.Entities;

public class EventType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    
    public List<Event> Events { get; set; } = new();
}