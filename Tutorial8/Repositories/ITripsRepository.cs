using Tutorial8.Models.Trip;

namespace Tutorial8.Repositories;

public interface ITripsRepository
{
    Task<List<Trip>> GetTripsAsync();
}