using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;

namespace Core.IntegrationTests.Infrastructure.Authentication;

public class TokenShould
{
    private const string Issuer = "TestIssuer";
    private const string Audience = "TestAudience";
    private const string Secret = "super secret key that should be stored in a secure place and not in code";
    private const string JwtPattern = @"^[A-Za-z0-9-_]+?\.[A-Za-z0-9-_]+?\.[A-Za-z0-9-_]+$";


    [Fact]
    public void BeCreated()
    {
        // arrange
        var regex = new Regex(JwtPattern);

        // act
        var token = CreateToken();

        // assert
        token.Should().NotBeEmpty();
        regex.IsMatch(token).Should().BeTrue();
    }

    [Fact]
    public void BeValidated()
    {
        // arrange
        var token = CreateToken();
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Secret));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateLifetime = true
        };
        var handler = new JwtSecurityTokenHandler();

        // act
        var principal = handler.ValidateToken(token, validationParameters, out _);

        // assert
        var claimsIdentity = new ClaimsIdentity(principal.Claims, "Bearer");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        claimsPrincipal.Identity?.IsAuthenticated.Should().BeTrue();
        claimsPrincipal.Identity?.AuthenticationType.Should().Be("Bearer");
        claimsPrincipal.Identity?.Name.Should().Be("Test User");
        claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Role).Value.Should().Be("Admin");
        claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Name).Value.Should().Be("Test User");
    }

    [Fact]
    public void NotBeValidBySigningKey()
    {
        // arrange
        var wrongSecret = "wrong secret key";
        var token = CreateToken();
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(wrongSecret));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
        };
        var handler = new JwtSecurityTokenHandler();

        // act
        Action act = () => handler.ValidateToken(token, validationParameters, out _);

        // assert
        act.Should().Throw<SecurityTokenSignatureKeyNotFoundException>();
    }

    [Fact]
    public void NotBeValidByAudience()
    {
        // arrange
        var token = CreateToken();
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Secret));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = "wrong audience",
        };
        var handler = new JwtSecurityTokenHandler();

        // act
        Action act = () => handler.ValidateToken(token, validationParameters, out _);

        // assert
        act.Should().Throw<SecurityTokenInvalidAudienceException>();
    }

    [Fact]
    public void NotBeValidByIssuer()
    {
        // arrange
        var token = CreateToken();
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Secret));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = "wrong issuer",
            ValidateAudience = true,
            ValidAudience = Audience,
        };
        var handler = new JwtSecurityTokenHandler();

        // act
        Action act = () => handler.ValidateToken(token, validationParameters, out _);

        // assert
        act.Should().Throw<SecurityTokenInvalidIssuerException>();
    }

    private string CreateToken()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "Test User"),
            new(ClaimTypes.Role, "Admin"),
        };

        // this symmetric key is used to both sign and verify the token 
        // a public and private key pair can be used instead for asymmetric encryption
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Secret));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: cred
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}