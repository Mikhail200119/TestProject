using System.Security.Claims;
using Event.Bll.Models;
using Event.Bll.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Event.Bll.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public User GetCurrentUser()
    {
        var claims = _httpContextAccessor.HttpContext?.User.Claims 
                     ?? throw new ApplicationException("User claims are null.");
        
        var email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ApplicationException("Token's claims does not contain any user data.");
        }

        return new User { Email = email };
    }
}