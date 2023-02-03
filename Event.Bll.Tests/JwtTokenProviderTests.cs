using System.Security.Claims;
using Event.Bll.Options;
using Event.Bll.Services;
using Event.Bll.Services.Interfaces;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Event.Bll.Tests;

public class JwtTokenProviderTests
{
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly Mock<SecurityTokenHandler> _mockSecurityTokenHandler;

    public JwtTokenProviderTests()
    {
        _mockSecurityTokenHandler = new Mock<SecurityTokenHandler>();

        var jwtOptions = new JwtOptions
        {
            EncryptionAlgorithms = new[] { "HS256" },
            ExpirationTime = TimeSpan.FromMinutes(2),
            SecretKey = Guid.NewGuid().ToString()
        };

        var options = Microsoft.Extensions.Options.Options.Create(jwtOptions);

        _jwtTokenProvider = new JwtTokenProvider(_mockSecurityTokenHandler.Object, options);
    }

    [Fact]
    public void Should_Successfully_Create_Jwt_Token()
    {
        // Arrange
        const string expectedToken = "ExpectedToken";
        var mockSecurityToken = new Mock<SecurityToken>();

        _mockSecurityTokenHandler
            .Setup(st => st.CreateToken(It.IsAny<SecurityTokenDescriptor>()))
            .Returns(mockSecurityToken.Object);

        _mockSecurityTokenHandler
            .Setup(st => st.WriteToken(mockSecurityToken.Object))
            .Returns(expectedToken);

        // Act
        var result = _jwtTokenProvider.CreateToken(new List<Claim>());

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectedToken);
        
        _mockSecurityTokenHandler.VerifyAll();
    }

    [Fact]
    public void Should_Throw_ArgumentNullException_When_Claims_Null()
    {
        // Act
        Action createTokenAction = () => _jwtTokenProvider.CreateToken(null);

        // Assert
        createTokenAction
            .Should()
            .ThrowExactly<ArgumentNullException>();
    }
}