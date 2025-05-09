using Microsoft.Data.SqlClient;
using Tutorial8.Models.Country;
using Tutorial8.Models.Trip;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsRepository;
    private readonly ICountriesRepository _countriesRepository;

    public TripsService(ITripsRepository tripsRepository,
        ICountriesRepository countriesRepository)
    {
        _tripsRepository = tripsRepository;
        _countriesRepository = countriesRepository;
    }

    public async Task<List<TripDTO>> GetTripsAsync()
    {
        var trips = await _tripsRepository.GetTripsAsync();

        var countryIds = trips
            .SelectMany(t => t.CountryTrips.Select(ct => ct.IdCountry))
            .Distinct()
            .ToList();

        var countries = await _countriesRepository.GetByIdsAsync(countryIds);
        var countryMap = countries.ToDictionary(c => c.IdCountry);

        return trips.Select(t => new TripDTO
            {
                Id = t.IdTrip,
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.CountryTrips
                    .Select(ct =>
                        new CountryDTO
                        {
                            Id = ct.IdCountry,
                            Name = countryMap[ct.IdCountry].Name
                        })
                    .ToList()
            })
            .ToList();
    }
}