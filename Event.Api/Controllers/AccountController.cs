using System.Security.Claims;
using Event.Api.Models.Request;
using Event.Api.Models.Response;
using Event.Bll.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Event.Api.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public AccountController(IJwtTokenProvider jwtTokenProvider)
    {
        _jwtTokenProvider = jwtTokenProvider;
    }

    [HttpPost("[action]")]
    public IActionResult Authenticate(AuthenticateRequest authenticateRequest)
    {
        if (!ModelState.IsValid)
        {
            throw new ApplicationException("Not valid user data.");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, authenticateRequest.UserEmail)
        };

        var response = new AuthenticateResponse
        {
            UserEmail = authenticateRequest.UserEmail,
            Token = _jwtTokenProvider.CreateToken(claims)
        };

        return Ok(response);
    }
}