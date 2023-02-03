using System.ComponentModel.DataAnnotations;

namespace Event.Api.Models.Request;

public class AuthenticateRequest
{
    [Required]
    public string UserEmail { get; set; }
}