using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Services;
using DawnWeaver.Application.Services.TimeManagement;
using MediatR;

namespace DawnWeaver.Application.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler(GetAllEventsService getEventsService, CollisionService collisionService, IAppDbContext context) : IRequestHandler<GetAllEventsQuery, AllEventsViewModel>
{
    public async Task<AllEventsViewModel> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var fullList = await getEventsService.GetAllEvents(request.From, request.To, cancellationToken);

        var outputInstances = collisionService.MarkCollision(fullList);
        
        return new AllEventsViewModel
        {
            Events = outputInstances
        };
    }
}