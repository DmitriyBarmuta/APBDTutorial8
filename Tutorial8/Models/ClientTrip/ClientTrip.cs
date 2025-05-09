namespace Tutorial8.Models.ClientTrip;

public class ClientTrip
{
    public int IdClient { get; set; }
    public int IdTrip { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime? PaymentDate { get; set; }
}