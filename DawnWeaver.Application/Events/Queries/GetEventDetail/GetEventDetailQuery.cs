using DawnWeaver.Domain.Common;
using MediatR;

namespace DawnWeaver.Application.Events.Queries.GetEventDetail;

public class GetEventDetailQuery : IRequest<ResultT<EventDetailViewModel>>
{
    public Guid EventId { get; set; }
}