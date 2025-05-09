namespace Tutorial8.Exceptions;

public class AlreadyRegisteredException : Exception
{
    public AlreadyRegisteredException(string? message) : base(message)
    {
    }
}