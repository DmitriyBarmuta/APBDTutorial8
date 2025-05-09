using Tutorial8.Models.ClientTrip;

namespace Tutorial8.Services;

public interface IClientsService
{
    Task<List<ClientTripDTO>> GetClientTripsAsync(int clientId);
}