using MediatR;

namespace DawnWeaver.Application.Events.Queries.GetEventDetail;

public class GetEventDetailQuery : IRequest<EventDetailViewModel>
{
    public Guid EventId { get; set; }
}