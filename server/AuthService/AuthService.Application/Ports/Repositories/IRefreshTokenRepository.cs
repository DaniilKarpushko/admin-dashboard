using AuthService.Domain;

namespace AuthService.Application.Ports.Repositories;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetRefreshTokenByToken(string token, CancellationToken cancellationToken);

    public Task<RefreshToken?> GetTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    public Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);

    public Task RemoveRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken);
}