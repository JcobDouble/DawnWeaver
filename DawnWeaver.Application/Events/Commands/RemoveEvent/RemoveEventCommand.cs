using MediatR;

namespace DawnWeaver.Application.Events.Commands.RemoveEvent;

public class RemoveEventCommand : IRequest
{
    public Guid EventId { get; set; }
}