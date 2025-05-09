namespace Tutorial8.Exceptions;

public class NoSuchTripException : Exception
{
    public NoSuchTripException(string? message) : base(message)
    {
    }
}