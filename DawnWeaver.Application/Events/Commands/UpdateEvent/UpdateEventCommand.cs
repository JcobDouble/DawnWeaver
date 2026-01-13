using DawnWeaver.Application.Events.Commands.AddEvent;
using DawnWeaver.Application.Events.Queries.GetEventDetail;
using DawnWeaver.Domain.Common;
using DawnWeaver.Domain.Enums;
using MediatR;

namespace DawnWeaver.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommand : IRequest<ResultT<EventDetailViewModel>>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? OccurrenceStart { get; set; }
    public bool? IsAllDay { get; set; }
    public bool? IsRecurring { get; set; }
    public int? DurationInMinutes { get; set; }
    public Guid? EventTypeId { get; set; }
    public required RecurrenceType RecurrenceType { get; set; }
    public RecurrenceDto? RecurrenceDto { get; set; }
}