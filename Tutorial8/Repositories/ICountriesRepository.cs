using Tutorial8.Models.Country;

namespace Tutorial8.Repositories;

public interface ICountriesRepository
{
    Task<List<Country>> GetByIdsAsync (IEnumerable<int> ids);
}