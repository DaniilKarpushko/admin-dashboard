using AuthService.Domain;

namespace AuthService.Application.Ports.Services;

public interface IAuthTokenService
{
    string GenerateAccessToken(User user, List<Role> userRoles);

    RefreshToken GenerateRefreshToken(User user);

    bool ValidateToken(string token);
}