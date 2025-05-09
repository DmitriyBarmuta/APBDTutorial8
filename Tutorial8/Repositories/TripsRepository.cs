using Microsoft.Data.SqlClient;
using Tutorial8.Infrastructure;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public TripsRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<TripDTO>> GetTrips()
    {
        var countriesDict = new Dictionary<int, TripDTO>();

        var sql = @"SELECT
                  t.IdTrip,
                  t.Name,
                  t.Description,
                  t.DateFrom,
                  t.DateTo,
                  t.MaxPeople,
                  c.IdCountry,
                  c.Name AS CountryName
                FROM Trip AS t
                LEFT JOIN Country_Trip ct ON t.IdTrip = ct.IdTrip
                LEFT JOIN Country c ON ct.IdCountry = c.IdCountry
                ORDER BY t.IdTrip;";

        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var tripId = reader.GetInt32(reader.GetOrdinal("IdTrip"));
            if (!countriesDict.TryGetValue(tripId, out var trip))
            {
                trip = new TripDTO
                {
                    Id = tripId,
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                    DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                    MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople")),
                    Countries = []
                };
                countriesDict[tripId] = trip;
            }

            var countryIdOrdinal = reader.GetOrdinal("IdCountry");
            if (await reader.IsDBNullAsync(countryIdOrdinal)) continue;
            var country = new CountryDTO
            {
                Id = reader.GetInt32(countryIdOrdinal),
                Name = reader.GetString(reader.GetOrdinal("CountryName"))
            };
            trip.Countries.Add(country);
        }

        return countriesDict.Values.ToList();
    }
}