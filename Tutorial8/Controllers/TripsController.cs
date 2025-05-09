using Microsoft.AspNetCore.Mvc;
using Tutorial8.Exceptions;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    public TripsController(ITripsService tripsService) => _tripsService = tripsService;

    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        try
        {
            var tripsList = await _tripsService.GetTripsAsync();
            return Ok(tripsList);
        }
        catch (DatabaseConnectionException ex)
        {
            return StatusCode(500, new { error = "Database error", detail = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Unexpected error", detail = ex.Message });
        }
    }
}