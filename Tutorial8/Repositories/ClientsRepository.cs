using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8.Infrastructure;
using Tutorial8.Models.ClientTrip;

namespace Tutorial8.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ClientsRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> DoesClientExistAsync(int clientId)
    {
        const string sql = "SELECT COUNT(*) FROM Client WHERE IdClient = @id";
        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = clientId });

        await conn.OpenAsync();
        var count = (int)(await cmd.ExecuteScalarAsync() ?? 0);
        return count > 0;
    }

    public async Task<List<ClientTrip>> GetClientTripsAsync(int clientId)
    {
        const string sql = """
                           SELECT IdClient, IdTrip, RegisteredAt, PaymentDate
                           FROM Client_Trip
                           WHERE IdClient = @id
                           ORDER BY RegisteredAt;
                           """;

        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = clientId });

        await conn.OpenAsync();

        var list = new List<ClientTrip>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new ClientTrip
            {
                IdClient = reader.GetInt32(reader.GetOrdinal("IdClient")),
                IdTrip = reader.GetInt32(reader.GetOrdinal("IdTrip")),
                RegisteredAt = reader.GetDateTime(reader.GetOrdinal("RegisteredAt")),
                PaymentDate = await reader.IsDBNullAsync(reader.GetOrdinal("PaymentDate"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("PaymentDate"))
            });
        }

        return list;
    }
}