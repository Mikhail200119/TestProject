namespace Event.Bll.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message)
    {
        Message = message;
    }
    
    public string Message { get; }
}