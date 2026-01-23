using DawnWeaver.Application.TimeManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DawnWeaver.Api.Controllers;

[Route("api/time-management")]
public class TimeManagementController : BaseController
{
    [HttpGet("proposals")]
    public async Task<ActionResult<List<DateTime>>> GetTimeProposals([FromQuery] int durationInMinutes)
    {
        var result = await Mediator.Send(new GetTimeManagementProposalQuery()
        {
            DurationInMinutes = durationInMinutes
        });
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
}