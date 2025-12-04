using DawnWeaver.Domain.Common;
using DawnWeaver.Domain.Enums;

namespace DawnWeaver.Domain.Entities;

public class Event : AuditableEntity
{
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsAllDay { get; set; } = false;
    public bool IsRecurring { get; set; } = false;
    public int DurationInMinutes { get; set; }
    
    public Guid EventTypeId { get; set; }
    public EventType EventType { get; set; } = null!;
    public EventStatus Status { get; set; }
}