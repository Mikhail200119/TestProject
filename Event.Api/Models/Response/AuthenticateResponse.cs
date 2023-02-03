namespace Event.Api.Models.Response;

public class AuthenticateResponse
{
    public string UserEmail { get; set; }
    public string Token { get; set; }
}