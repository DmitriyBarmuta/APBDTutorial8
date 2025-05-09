using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8.Infrastructure;
using Tutorial8.Models.CountryTrip;
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
                    CountryTrips = []
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

    public async Task<List<Trip>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0)
            return [];

        var parameters = idList
            .Select((id, idx) => new SqlParameter($"@p{idx}", SqlDbType.Int) { Value = id })
            .ToArray();
        var inClause = string.Join(", ", parameters.Select(p => p.ParameterName));

        var sql = $"""
                   SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople
                   FROM Trip
                   WHERE IdTrip IN ({inClause});
                   """;

        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddRange(parameters);

        await conn.OpenAsync();

        var result = new List<Trip>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new Trip
            {
                IdTrip = reader.GetInt32(reader.GetOrdinal("IdTrip")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople")),
                CountryTrips = []
            });
        }

        return result;
    }

    public async Task<bool> DoesTripExistAsync(int tripId)
    {
        const string sql = "SELECT Count(*) From Trip WHERE IdTrip = @tripId";

        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@tripId", tripId);

        await conn.OpenAsync();
        var result = (int)(await cmd.ExecuteScalarAsync() ?? 0);
        return result > 0;
    }
}