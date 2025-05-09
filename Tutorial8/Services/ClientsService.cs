using Tutorial8.Exceptions;
using Tutorial8.Models.Client;
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
        if (clientId <= 0) throw new InvalidClientIdException("Client ID must be a positive integer.");

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

    public async Task<int> CreateClientAsync(CreateClientDTO createClientDto)
    {
        return await _clientsRepository.CreateClientAsync(createClientDto);
    }

    public async Task RegisterClientToTripAsync(int clientId, int tripId)
    {
        if (clientId <= 0) throw new InvalidClientIdException("Client ID must be a positive integer.");
        if (tripId <= 0) throw new InvalidTripIdException("Trip ID must be a positive integer.");
        
        if (!await _clientsRepository.DoesClientExistAsync(clientId)) 
            throw new NoSuchClientException($"Client with ID {clientId} not found.");

        if (!await _tripsRepository.DoesTripExistAsync(tripId))
            throw new NoSuchTripException($"Trip with ID {tripId} not found.");

        if (await _clientsRepository.DoesClientExistAsync(clientId))
            throw new AlreadyRegisteredException(
                $"Client with ID {clientId} already registered for trip with ID {tripId}");

        var trip = await _tripsRepository.GetByIdAsync(tripId);
        var registrationsCount = await _tripsRepository.CountTripRegistrationsAsync(tripId);

        if (registrationsCount >= trip!.MaxPeople)
            throw new TripFullException($"Trip with ID {tripId} is full.");

        var inserted = await _clientsRepository.RegisterClientForTripAsync(clientId, tripId);
        if (!inserted)
            throw new DatabaseConnectionException($"Failed to register client for trip with ID {tripId}");
    }

    public async Task RemoveClientFromTripAsync(int clientId, int tripId)
    {
        if (clientId <= 0) throw new InvalidClientIdException("Client ID must be a positive integer.");
        if (tripId <= 0) throw new InvalidTripIdException("Trip ID must be a positive integer.");
        
        if (!await _clientsRepository.DoesClientExistAsync(clientId)) 
            throw new NoSuchClientException($"Client with ID {clientId} not found.");

        if (!await _tripsRepository.DoesTripExistAsync(tripId))
            throw new NoSuchTripException($"Trip with ID {tripId} not found.");

        var deleted = await _clientsRepository.DeleteClientTripAsync(clientId, tripId);
        if (!deleted)
            throw new RegistrationNotFoundException($"Registration of client {clientId} to trip {tripId} not found.");
    }
}