namespace Tutorial8.Exceptions;

public class InvalidTripIdException : Exception
{
    public InvalidTripIdException(string? message) : base(message)
    {
    }
}