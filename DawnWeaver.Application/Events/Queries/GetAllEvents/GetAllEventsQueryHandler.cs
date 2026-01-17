using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Services;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler(GetAllEventsService service, IAppDbContext context) : IRequestHandler<GetAllEventsQuery, AllEventsViewModel>
{
    public async Task<AllEventsViewModel> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var outputInstances = await service.GetAllEvents(request.From, request.To, cancellationToken);

        return new AllEventsViewModel
        {
            Events = outputInstances
        };
    }
}