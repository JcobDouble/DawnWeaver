using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Common.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Queries.GetEventDetail;

public class GetEventDetailQueryHandler(IAppDbContext context) : IRequestHandler<GetEventDetailQuery, EventDetailViewModel>
{
    public async Task<EventDetailViewModel> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
    {
        var eventInDb = await context.Events.FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);
        
        if (eventInDb == null)
        {
            throw new Exception("Event not found");
        }

        var eventDetail = eventInDb.MapToEventDetailViewModel();

        return eventDetail;
    }
}