using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Event.Api.Controllers;
using Event.Api.Models.Request;
using Event.Api.Models.Response;
using Event.Bll.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Event.Api.Tests;

public class AccountControllerTests
{
    private readonly AccountController _accountController;
    private readonly Mock<IJwtTokenProvider> _mockTokenProvider;

    public AccountControllerTests()
    {
        _mockTokenProvider = new Mock<IJwtTokenProvider>();
        _accountController = new AccountController(_mockTokenProvider.Object);
    }

    [Fact]
    public void Should_Return_OkResult_In_Authentication()
    {
        // Arrange
        const string userEmail = "UserEmail";
        const string expectedToken = "token";
        var authenticateRequest = new AuthenticateRequest { UserEmail = userEmail };

        _mockTokenProvider
            .Setup(tp => tp.CreateToken(It.IsAny<IEnumerable<Claim>>()))
            .Returns(expectedToken);

        // Act
        var result = _accountController.Authenticate(authenticateRequest);

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>().Which.Value
            .Should()
            .BeEquivalentTo(new AuthenticateResponse
            {
                UserEmail = userEmail,
                Token = expectedToken
            });

        _mockTokenProvider.VerifyAll();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("       ")]
    [InlineData("")]
    public void Should_Throw_Exception_When_AuthenticateRequest_In_Not_Valid(string userEmail)
    {
        // Arrange
        var authenticateRequest = new AuthenticateRequest { UserEmail = userEmail };

        // Act
        var isValid = IsModelValid(authenticateRequest);

        // Assert
        isValid
            .Should()
            .BeFalse();
    }

    private static bool IsModelValid(AuthenticateRequest authenticateRequest)
    {
        var validationContext = new ValidationContext(authenticateRequest);
        
        return Validator.TryValidateObject(authenticateRequest, validationContext, new List<ValidationResult>());
    }
}