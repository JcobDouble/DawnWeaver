using DawnWeaver.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler(IAppDbContext context) : IRequestHandler<GetAllEventsQuery, AllEventsViewModel>
{
    public async Task<AllEventsViewModel> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var eventsInDb = await context.Events.ToListAsync(cancellationToken);
        
        var allEvents = new AllEventsViewModel()
        {
            Events = eventsInDb.Where(e => e.StartDate is not null && e.EndDate is not null).Select(e => new AllEventsDto
            {
                Id = e.Id,
                Title = e.Title,
                StartDate = e.StartDate!.Value,
                EndDate = e.EndDate!.Value
            }).ToList()
        };
        
        return allEvents;
    }
}