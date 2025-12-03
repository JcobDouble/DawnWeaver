using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DawnWeaver.Api.Controllers;

[EnableCors("MyAllowSpecificOrigins")]
[Route("/api/hc")]
public class HealthChecksController : BaseController
{
    /// <summary>
    /// Health check endpoint
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<string>> GetAsync()
    {
        return "Healthy";
    }
}