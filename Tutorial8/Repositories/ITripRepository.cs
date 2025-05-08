using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public interface ITripRepository
{
    Task<List<TripDTO>> GetTrips();
}