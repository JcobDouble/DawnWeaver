using DawnWeaver.Application.Events.Queries.GetAllEvents;

namespace DawnWeaver.Application.Services.TimeManagement;

public class CollisionService
{
    public List<AllEventsDto> MarkCollision(List<AllEventsDto> existingEvents)
    {
        var sortedEvents = existingEvents
            .OrderBy(e => e.StartDate)
            .Select(e => new AllEventsDto
            {
                Id = e.Id,
                Title = e.Title,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                HasConflict = false
            })
            .ToList();
        
        var maxEnd = sortedEvents[0].EndDate;
        var maxEndIndex = 0;
        
        for (int i = 1; i < sortedEvents.Count; i++)
        {
            if (sortedEvents[i].StartDate < maxEnd)
            {
                sortedEvents[i].HasConflict = true;
                sortedEvents[maxEndIndex].HasConflict = true;
            }
            
            if (sortedEvents[i].EndDate > maxEnd)
            {
                maxEnd = sortedEvents[i].EndDate;
                maxEndIndex = i;
            }
        }
        
        return sortedEvents;
    }
}