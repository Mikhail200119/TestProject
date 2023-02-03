namespace Event.Bll.Exceptions;

public class PermissionsException : Exception
{
    public PermissionsException(string? message) : base(message)
    {
    }

    public string Message { get; }
}