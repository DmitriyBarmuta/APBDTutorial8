using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsRepository;

    public TripsService(ITripsRepository tripsRepository)
    {
        this._tripsRepository = tripsRepository;
    }

    public async Task<List<TripDTO>> GetTrips() => await _tripsRepository.GetTrips();

    /*var trips = new List<TripDTO>();

    string command = "SELECT IdTrip, Name FROM Trip";

    using (SqlConnection conn = new SqlConnection())
    using (SqlCommand cmd = new SqlCommand(command, conn))
    {
        await conn.OpenAsync();

        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                int idOrdinal = reader.GetOrdinal("IdTrip");
                trips.Add(new TripDTO()
                {
                    Id = reader.GetInt32(idOrdinal),
                    Name = reader.GetString(1),
                });
            }
        }
    }


    return trips;*/
}