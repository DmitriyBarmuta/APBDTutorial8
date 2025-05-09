using Tutorial8.Infrastructure;
using Tutorial8.Models.Country;
using Tutorial8.Models.Join;
using Tutorial8.Models.Trip;
using Tutorial8.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public TripsRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Trip>> GetTripsAsync()
    {
        var tripsDict = new Dictionary<int, Trip>();

        const string sql = """
                           SELECT
                           t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople,
                           c.IdCountry, c.Name AS CountryName
                           FROM Trip t
                           LEFT JOIN Country_Trip ct ON t.IdTrip = ct.IdTrip
                           LEFT JOIN Country c ON ct.IdCountry = c.IdCountry
                           ORDER BY t.IdTrip;
                           """;

        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        await conn.OpenAsync();

        await using var rdr = await cmd.ExecuteReaderAsync();
        while (await rdr.ReadAsync())
        {
            var id = rdr.GetInt32(rdr.GetOrdinal("IdTrip"));

            if (!tripsDict.TryGetValue(id, out var trip))
            {
                trip = new Trip
                {
                    IdTrip = id,
                    Name = rdr.GetString(rdr.GetOrdinal("Name")),
                    Description = rdr.GetString(rdr.GetOrdinal("Description")),
                    DateFrom = rdr.GetDateTime(rdr.GetOrdinal("DateFrom")),
                    DateTo = rdr.GetDateTime(rdr.GetOrdinal("DateTo")),
                    MaxPeople = rdr.GetInt32(rdr.GetOrdinal("MaxPeople")),
                    CountryTrips = new List<CountryTrip>()
                };
                tripsDict[id] = trip;
            }

            var countryOrd = rdr.GetOrdinal("IdCountry");
            if (!await rdr.IsDBNullAsync(countryOrd))
            {
                trip.CountryTrips.Add(new CountryTrip
                {
                    IdCountry = rdr.GetInt32(countryOrd),
                    IdTrip = id
                });
            }
        }

        return tripsDict.Values.ToList();
    }
}