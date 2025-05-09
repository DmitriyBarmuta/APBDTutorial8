using Tutorial8.Exceptions;
using Tutorial8.Models.ClientTrip;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;
    private readonly ITripsRepository _tripsRepository;

    public ClientsService(IClientsRepository clientsRepository,
        ITripsRepository tripsRepository)
    {
        _clientsRepository = clientsRepository;
        _tripsRepository = tripsRepository;
    }

    public async Task<List<ClientTripDTO>> GetClientTripsAsync(int clientId)
    {
        if (clientId <= 0)
            throw new InvalidClientIdException("Client ID must be a positive integer.");

        var exists = await _clientsRepository.DoesClientExistAsync(clientId);
        if (!exists)
            throw new NoSuchClientException($"Client with ID {clientId} not found.");

        var registrations = await _clientsRepository.GetClientTripsAsync(clientId);

        var tripIds = registrations.Select(clientTrip => clientTrip.IdTrip).Distinct();
        var trips = await _tripsRepository.GetByIdsAsync(tripIds);
        var tripDict = trips.ToDictionary(trip => trip.IdTrip);

        return registrations.Select(clientTrip => new ClientTripDTO
            {
                IdTrip = clientTrip.IdTrip,
                Name = tripDict[clientTrip.IdTrip].Name,
                Description = tripDict[clientTrip.IdTrip].Description,
                DateFrom = tripDict[clientTrip.IdTrip].DateFrom,
                DateTo = tripDict[clientTrip.IdTrip].DateTo,
                MaxPeople = tripDict[clientTrip.IdTrip].MaxPeople,
                RegisteredAt = clientTrip.RegisteredAt,
                PaymentDate = clientTrip.PaymentDate
            })
            .ToList();
    }
}