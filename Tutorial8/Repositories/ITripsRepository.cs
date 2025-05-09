using Tutorial8.Models.Trip;

namespace Tutorial8.Repositories;

public interface ITripsRepository
{
    Task<List<Trip>> GetTripsAsync();
    Task<Trip?> GetByIdAsync(int id);
    Task<List<Trip>> GetByIdsAsync(IEnumerable<int> ids);
    Task<bool> DoesTripExistAsync(int id);
    Task<int> CountTripRegistrationsAsync(int id);
}