namespace Event.Api.Models.Response;

public class DevExceptionResponse : ExceptionResponse
{
    public string Type { get; set; }
    public string StackTrace { get; set; }
    public Exception InnerException { get; set; }
}