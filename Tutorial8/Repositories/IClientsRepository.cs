using Tutorial8.Models.ClientTrip;

namespace Tutorial8.Repositories;

public interface IClientsRepository
{
    Task<bool> DoesClientExistAsync(int clientId);

    Task<List<ClientTrip>> GetClientTripsAsync(int clientId);
}