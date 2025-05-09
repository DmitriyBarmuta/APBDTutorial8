using Microsoft.AspNetCore.Mvc;
using Tutorial8.Exceptions;
using Tutorial8.Models.Client;
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

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientDTO createClientDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var id = await _clientsService.CreateClientAsync(createClientDto);
        return CreatedAtAction(nameof(CreateClient), new { id }, new { id });
    }

    [HttpPut("{clientId:int}/trips/{tripId:int}")]
    public async Task<IActionResult> RegisterClientToTrip(int clientId, int tripId)
    {
        try
        {
            await _clientsService.RegisterClientToTripAsync(clientId, tripId);
            return NoContent();
        }
        catch (Exception e) when (e is NoSuchClientException or NoSuchTripException)
        {
            return BadRequest(new { error = e.Message });
        }
        catch (Exception e) when (e is InvalidClientIdException or InvalidTripIdException)
        {
            return NotFound(new { error = e.Message });
        }
        catch (Exception e) when (e is AlreadyRegisteredException or TripFullException)
        {
            return Conflict(new { error = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = "An unexpected error occurred.", detail = e.Message });
        }
    }

    [HttpDelete("{clientId:int}/trips/{tripId:int}")]
    public async Task<IActionResult> DeleteClientTrip(int clientId, int tripId)
    {
        try
        {
            await _clientsService.DeleteClientFromTripAsync(clientId, tripId);
            return NoContent();
        }
        catch (Exception e) when (e is NoSuchClientException or NoSuchTripException)
        {
            return BadRequest(new { error = e.Message });
        }
        catch (Exception e) when (e is InvalidClientIdException or InvalidTripIdException or RegistrationNotFoundException)
        {
            return NotFound(new { error = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = "An unexpected error occurred.", detail = e.Message });
        }
    }
}