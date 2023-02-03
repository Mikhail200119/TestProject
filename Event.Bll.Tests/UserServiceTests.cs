using System.Security.Claims;
using Event.Bll.Models;
using Event.Bll.Services;
using Event.Bll.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Event.Bll.Tests;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public UserServiceTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _userService = new UserService(_mockHttpContextAccessor.Object);
    }

    [Fact]
    public void Should_Successfully_Create_User_Object()
    {
        // Arrange
        const string userEmail = "TestEmail";

        _mockHttpContextAccessor
            .Setup(accessor => accessor.HttpContext.User.Claims)
            .Returns(new List<Claim> { new(ClaimTypes.Name, userEmail) });

        // Act
        var user = _userService.GetCurrentUser();

        // Assert
        user
            .Should()
            .BeEquivalentTo(new User { Email = userEmail });
    }

    [Fact]
    public void Should_Throw_ApplicationException_When_User_Clams_Null()
    {
        // Arrange
        _mockHttpContextAccessor
            .Setup(accessor => accessor.HttpContext.User.Claims)
            .Returns((IEnumerable<Claim>)null);

        // Act
        Action getCurrentUserAction = () => _userService.GetCurrentUser();

        // Assert
        getCurrentUserAction
            .Should()
            .Throw<ApplicationException>()
            .WithMessage("User claims are null.");
    }

    [Fact]
    public void Should_Throw_ApplicationException_When_Token_Claims_Doesnt_Contain_User_Data()
    {
        // Arrange
        _mockHttpContextAccessor
            .Setup(accessor => accessor.HttpContext.User.Claims)
            .Returns(new List<Claim>());

        // Act
        Action getCurrentUserAction = () => _userService.GetCurrentUser();

        // Assert
        getCurrentUserAction
            .Should()
            .Throw<ApplicationException>()
            .WithMessage("Token's claims does not contain any user data.");
    }
}