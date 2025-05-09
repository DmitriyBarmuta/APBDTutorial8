using Tutorial8.Models.Trip;

namespace Tutorial8.Services;

public interface ITripsService
{
    Task<List<TripDTO>> GetTripsAsync();
}