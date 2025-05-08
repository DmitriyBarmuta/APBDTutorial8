using System.Reflection;
using Microsoft.Data.SqlClient;

namespace Tutorial8.Config;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public async Task InitializeAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();


        var createSql = ReadEbmeddedSql("DatabaseCreate.sql");
        var fillSql = ReadEbmeddedSql("DatabaseFill.sql");

        using var createCommand = connection.CreateCommand();
        createCommand.CommandText = createSql;
        await createCommand.ExecuteNonQueryAsync();

        using var fillCommand = connection.CreateCommand();
        fillCommand.CommandText = fillSql;
        await fillCommand.ExecuteNonQueryAsync();
    }

    private string ReadEbmeddedSql(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly
            .GetManifestResourceNames()
            .FirstOrDefault(x => x.Contains(fileName));

        if (resourceName == null)
            throw new FileNotFoundException($"Couldn't find embedded resource: {fileName}");

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}