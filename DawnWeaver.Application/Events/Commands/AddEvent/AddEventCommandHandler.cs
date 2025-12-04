using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Common.Mapping;
using DawnWeaver.Application.Events.Queries.GetEventDetail;
using DawnWeaver.Domain.Entities;
using DawnWeaver.Domain.Enums;
using MediatR;

namespace DawnWeaver.Application.Events.Commands.AddEvent;

public class AddEventCommandHandler(IAppDbContext context) : IRequestHandler<AddEventCommand, EventDetailViewModel>
{
    public async Task<EventDetailViewModel> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Event
        {
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsAllDay = request.IsAllDay,
            IsRecurring = request.IsRecurring,
            DurationInMinutes = request.DurationInMinutes,
            EventTypeId = request.EventTypeId,
            EventType = context.EventTypes.FirstOrDefault(e => e.Id == request.EventTypeId)!,
            Status = EventStatus.Confirmed
        };
        
        context.Events.Add(newEvent);
        await context.SaveChangesAsync(cancellationToken);
        return newEvent.MapToEventDetailViewModel();
    }
}