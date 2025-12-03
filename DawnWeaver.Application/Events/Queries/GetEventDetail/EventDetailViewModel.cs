using DawnWeaver.Domain.Entities;

namespace DawnWeaver.Application.Events.Queries.GetEventDetail;

public class EventDetailViewModel
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAllDay { get; set; } = false;
    public bool IsRecurring { get; set; } = false;
    public int DurationInMinutes { get; set; }
    public Guid EventTypeId { get; set; }
    public required string EventType { get; set; }
}