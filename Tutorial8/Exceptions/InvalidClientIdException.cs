namespace Tutorial8.Exceptions;

public class InvalidClientIdException : Exception
{
    public InvalidClientIdException(string? message) : base(message)
    {
    }
}