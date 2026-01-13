using DawnWeaver.Domain.Common;
using DawnWeaver.Domain.Enums;
using MediatR;

namespace DawnWeaver.Application.Events.Commands.RemoveEvent;

public class RemoveEventCommand : IRequest<Result>
{
    public Guid EventId { get; set; }
    public DateTime OccurrenceDate { get; set; }
    public required RecurrenceType RecurrenceType { get; set; }
}