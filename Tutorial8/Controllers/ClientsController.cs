using Microsoft.AspNetCore.Mvc;
using Tutorial8.Exceptions;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet("{id:int}/trips")]
    public async Task<IActionResult> GetClientTrips(int id)
    {
        try
        {
            var trips = await _clientsService.GetClientTripsAsync(id);
            return Ok(trips);
        }
        catch (NoSuchClientException e)
        {
            return BadRequest(new { error = e.Message });
        }
        catch (InvalidClientIdException e)
        {
            return NotFound(new { error = e.Message });
        }
        catch (DatabaseConnectionException e)
        {
            return StatusCode(500, new { error = "A database error occurred.", detail = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = "An unexpected error occurred.", detail = e.Message });
        }
    }
}