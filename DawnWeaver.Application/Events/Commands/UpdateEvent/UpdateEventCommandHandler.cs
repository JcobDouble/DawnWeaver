using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Common.Mapping;
using DawnWeaver.Application.Events.Queries.GetEventDetail;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler(IAppDbContext context) :IRequestHandler<UpdateEventCommand, EventDetailViewModel>
{
    public async Task<EventDetailViewModel> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventInDb = await context.Events.Include(e => e.EventType).FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (eventInDb == null)
        {
            throw new Exception("Event not found");
        }
        
        var endDate = request.StartDate.AddMinutes(request.DurationInMinutes);
        
        eventInDb.Title = request.Title;
        eventInDb.Description = request.Description;
        eventInDb.StartDate = request.StartDate;
        eventInDb.EndDate = endDate;
        eventInDb.IsAllDay = request.IsAllDay;
        eventInDb.IsRecurring = request.IsRecurring;
        eventInDb.DurationInMinutes = request.DurationInMinutes;
        eventInDb.EventTypeId = request.EventTypeId;
        
        await context.SaveChangesAsync(cancellationToken);
        
        return eventInDb.MapToEventDetailViewModel();
    }
}