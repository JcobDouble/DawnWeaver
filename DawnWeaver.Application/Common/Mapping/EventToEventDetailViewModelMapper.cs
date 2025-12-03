using DawnWeaver.Application.Events.Queries.GetEventDetail;
using DawnWeaver.Domain.Entities;

namespace DawnWeaver.Application.Common.Mapping;

public static class EventToEventDetailViewModelMapper
{
    public static EventDetailViewModel MapToEventDetailViewModel(this Event calendarEvent)
    {
        return new EventDetailViewModel
        {
            Id = calendarEvent.Id,
            Title = calendarEvent.Title,
            Description = calendarEvent.Description,
            StartDate = calendarEvent.StartDate,
            EndDate = calendarEvent.EndDate,
            IsAllDay = calendarEvent.IsAllDay,
            IsRecurring = calendarEvent.IsRecurring,
            DurationInMinutes = calendarEvent.DurationInMinutes,
            EventTypeId = calendarEvent.EventTypeId,
            EventType = calendarEvent.EventType.Name
        };
    }
}