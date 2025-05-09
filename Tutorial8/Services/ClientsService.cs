using Tutorial8.Exceptions;
using Tutorial8.Models.Trip;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;

    public ClientsService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task<List<TripDTO>> GetClientTrips(int clientId)
    {
        if (clientId <= 0) throw new InvalidClientIdException("Client ID must be a positive integer.");

        return null;
    }
}