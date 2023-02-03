using System.Security.Claims;

namespace Event.Bll.Services.Interfaces;

public interface IJwtTokenProvider
{
    string CreateToken(IEnumerable<Claim> claims);
}