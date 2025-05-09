namespace Tutorial8.Exceptions;

public class RegistrationNotFoundException : Exception
{
    public RegistrationNotFoundException(string? message) : base(message)
    {
    }
}