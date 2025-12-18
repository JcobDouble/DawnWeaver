using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Common.Mapping;
using DawnWeaver.Application.Events.Queries.GetEventDetail;
using DawnWeaver.Domain.Entities;
using DawnWeaver.Domain.Enums;
using Ical.Net.DataTypes;
using MediatR;

namespace DawnWeaver.Application.Events.Commands.AddEvent;

public class AddEventCommandHandler(IAppDbContext context) : IRequestHandler<AddEventCommand, EventDetailViewModel>
{
    public async Task<EventDetailViewModel> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        var endDate = request.StartDate.AddMinutes(request.DurationInMinutes);
        
        var newEvent = new Event
        {
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = endDate,
            IsAllDay = request.IsAllDay,
            DurationInMinutes = request.DurationInMinutes,
            EventTypeId = request.EventTypeId,
            EventType = context.EventTypes.FirstOrDefault(e => e.Id == request.EventTypeId)!,
            Status = EventStatus.Confirmed
        };
        
        if (request.Recurrence != null)
        {
            newEvent.IsRecurring = true;
            newEvent.RRule = RRuleBuilder.BuildRRule(request.Recurrence);
        }
        
        context.Events.Add(newEvent);
        await context.SaveChangesAsync(cancellationToken);
        return newEvent.MapToEventDetailViewModel();
    }
    
    
}