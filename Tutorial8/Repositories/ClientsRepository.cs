using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8.Infrastructure;
using Tutorial8.Models.Client;
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

    public async Task<int> CreateClientAsync(CreateClientDTO createClientDto)
    {
        const string sql = """
                           INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                           OUTPUT INSERTED.IdClient
                           VALUES (@fname, @lname, @email, @telephone, @pesel);
                           """;

        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@fname", createClientDto.FirstName);
        cmd.Parameters.AddWithValue("@lname", createClientDto.LastName);
        cmd.Parameters.AddWithValue("@email", createClientDto.Email);
        cmd.Parameters.AddWithValue("@telephone", createClientDto.Telephone);
        cmd.Parameters.AddWithValue("@pesel", createClientDto.Pesel);

        await conn.OpenAsync();
        return (int)(await cmd.ExecuteScalarAsync() ?? -1);
    }

    public async Task<bool> IsClientRegisteredForTripAsync(int clientId, int tripId)
    {
        const string sql = "SELECT COUNT(*) FROM Client_Trip WHERE IdClient = @clientId AND IdTrip = @tripId";
        
        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@clientId", clientId);
        cmd.Parameters.AddWithValue("@tripId", tripId);

        await conn.OpenAsync();
        var count = (int)(await cmd.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

    public async Task RegisterClientForTripAsync(int clientId, int tripId)
    {
        var registeredAt = DateTime.UtcNow;
        const string sql = @"INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt) VALUES
                       (@clientId, @tripId, @registeredAt)";
        
        await using var conn = _connectionFactory.GetConnection();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@clientId", clientId);
        cmd.Parameters.AddWithValue("@tripId", tripId);
        cmd.Parameters.AddWithValue("@registeredAt", registeredAt);
        
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public Task<bool> DeleteClientTripAsync(int clientId, int tripId)
    {
        throw new NotImplementedException();
    }
}