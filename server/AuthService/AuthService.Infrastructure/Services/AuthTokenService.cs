using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Domain;
using AuthService.Application.Ports.Services;
using AuthService.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Services;

public class AuthTokenService : IAuthTokenService
{
    private readonly JwtTokenOptions _tokenOptions;

    public AuthTokenService(IOptions<JwtTokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public string GenerateAccessToken(User user, List<Role> userRoles)
    {
        var key = _tokenOptions.Key;
        var tokenExpiration = _tokenOptions.AccessTokenExpiration;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
        };

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenExpiration),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256Signature));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(User user)
    {
        var tokenExpiration = _tokenOptions.RefreshTokenExpiration;

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.UtcNow.AddMinutes(tokenExpiration),
            UserId = user.Id,
        };

        return refreshToken;
    }

    public bool ValidateToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key)),
            ClockSkew = TimeSpan.FromMinutes(0)
        };

        var jwtHandler = new JwtSecurityTokenHandler();
        try
        {
            jwtHandler.ValidateToken(token, tokenValidationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}