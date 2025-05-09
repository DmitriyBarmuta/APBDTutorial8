using Tutorial8.Models.Client;
using Tutorial8.Models.ClientTrip;

namespace Tutorial8.Services;

public interface IClientsService
{
    Task<List<ClientTripDTO>> GetClientTripsAsync(int clientId);
    Task<int> CreateClientAsync(CreateClientDTO createClientDto);
    Task RegisterClientToTripAsync(int clientId, int tripId);
    Task DeleteClientFromTripAsync(int clientId, int tripId);
}