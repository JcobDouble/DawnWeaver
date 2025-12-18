using MediatR;

namespace DawnWeaver.Application.Events.Queries.GetAllEvents;

public class GetAllEventsQuery : IRequest<AllEventsViewModel>
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}