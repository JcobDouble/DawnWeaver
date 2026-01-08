using DawnWeaver.Application.Events.Commands.AddEvent;
using DawnWeaver.Application.Events.Commands.RemoveEvent;
using DawnWeaver.Application.Events.Commands.UpdateEvent;
using DawnWeaver.Application.Events.Queries.GetAllEvents;
using DawnWeaver.Application.Events.Queries.GetEventDetail;
using DawnWeaver.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DawnWeaver.Api.Controllers;

[Route("api/events")]
public class EventsController : BaseController
{
    [HttpGet("{eventId}")]
    public async Task<ActionResult<EventDetailViewModel>> GetEventDetail(Guid eventId)
    {
        var result = await Mediator.Send(new GetEventDetailQuery
        {
            EventId = eventId
        });
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<AllEventsViewModel>> GetAllEvents([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var query = new GetAllEventsQuery
        {
            From = from,
            To = to
        };
        
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }
    
    [HttpPost("add")]
    public async Task<ActionResult<EventDetailViewModel>> AddEvent(AddEventCommand command)
    {
        var result = await Mediator.Send(command);
        
        return Ok(result);
    }

    [HttpPut("update/{eventId}")]
    public async Task<ActionResult<EventDetailViewModel>> UpdateEvent(Guid eventId, UpdateEventCommand command)
    {
        if (eventId != command.Id)
        {
            return BadRequest("Route ID does not match body ID.");
        }
        
        var result = await Mediator.Send(command);
        
        return Ok(result);
    }

    [HttpDelete("remove/{eventId}")]
    public async Task<ActionResult> RemoveEvent(Guid eventId, [FromQuery] DateTime occurrenceDate, [FromQuery] RecurrenceType recurrenceType)
    {
        await Mediator.Send(new RemoveEventCommand
        {
            EventId = eventId,
            OccurrenceDate = occurrenceDate,
            RecurrenceType = recurrenceType
        });

        return Ok();
    }
}