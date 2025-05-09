namespace Tutorial8.Models.Trip;

public class Trip
{
    public int IdTrip { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }

    public ICollection<CountryTrip.CountryTrip> CountryTrips { get; set; }
    public ICollection<ClientTrip.ClientTrip> ClientTrips { get; set; }
}