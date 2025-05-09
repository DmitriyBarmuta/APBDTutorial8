namespace Tutorial8.Exceptions;

public class TripFullException : Exception
{
    public TripFullException(string? message) : base(message)
    {
    }
}