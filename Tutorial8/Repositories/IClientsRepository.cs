using Tutorial8.Models.Client;
using Tutorial8.Models.ClientTrip;

namespace Tutorial8.Repositories;

// IClientsRepository.cs
public interface IClientsRepository
{
    Task<int> CreateClientAsync(CreateClientDTO dto);
    Task<bool> DoesClientExistAsync(int clientId);
    Task<List<ClientTrip>> GetClientTripsAsync(int clientId);
    Task<bool> IsClientRegisteredForTripAsync(int clientId, int tripId);
    Task<bool> RegisterClientForTripAsync(int clientId, int tripId);
    Task<bool> DeleteClientTripAsync(int clientId, int tripId);
}