using DawnWeaver.Application.Events.Queries.GetEventDetail;
using MediatR;

namespace DawnWeaver.Application.Events.Commands.AddEvent;

public class AddEventCommand : IRequest<EventDetailViewModel>
{
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public required DateTime StartDate { get; set; }
    public bool IsAllDay { get; set; } = false;
    public bool IsRecurring { get; set; } = false;
    public required int DurationInMinutes { get; set; }
    public Guid EventTypeId { get; set; }
}