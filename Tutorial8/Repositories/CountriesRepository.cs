using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8.Infrastructure;
using Tutorial8.Models.Country;

namespace Tutorial8.Repositories;

public class CountriesRepository : ICountriesRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CountriesRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Country>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0)
            return [];

        var parameters = idList
            .Select((id, idx) => new SqlParameter($"@p{idx}", SqlDbType.Int) { Value = id })
            .ToArray();

        var inClause = string.Join(", ", parameters.Select(p => p.ParameterName));

        var sql = $@"
                SELECT IdCountry, Name
                FROM Country
                WHERE IdCountry IN ({inClause});";

        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddRange(parameters);

        await conn.OpenAsync();

        var results = new List<Country>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(new Country
            {
                IdCountry = reader.GetInt32(reader.GetOrdinal("IdCountry")),
                Name = reader.GetString(reader.GetOrdinal("Name"))
            });
        }

        return results;
    }
}