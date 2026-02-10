using DawnWeaver.Application.Events.Queries.GetAllEvents;
using DawnWeaver.Domain.Common;
using DawnWeaver.Domain.Enums;
using MediatR;

namespace DawnWeaver.Application.Events.Queries.CheckForCollisions;

public class CheckForCollisionsQuery : IRequest<ResultT<AllEventsViewModel>>
{
    // przemyśleć które mogą być null
    
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public int DurationInMinutes { get; set; }
    public DateTime OriginalStartDate { get; set; }
    public string RRule { get; set; } = string.Empty;
    public RecurrenceType RecurrenceType { get; set; }
}