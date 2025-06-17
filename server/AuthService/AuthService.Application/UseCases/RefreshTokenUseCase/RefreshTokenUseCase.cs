using System.Transactions;
using AuthService.Application.Ports.Repositories;
using AuthService.Application.Ports.Services;
using AuthService.Domain;

namespace AuthService.Application.UseCases.RefreshTokenUseCase;

public class RefreshTokenUseCase : IRefreshTokenUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthTokenService _authTokenService;

    public RefreshTokenUseCase(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IAuthTokenService authTokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _authTokenService = authTokenService;
    }

    public async Task<RefreshTokenResponse> ExecuteAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!Guid.TryParse(request.RefreshToken, out _))
                return new RefreshTokenResponse.InvalidRefreshToken("Invalid refresh token ID.");

            var refreshToken = await _refreshTokenRepository.GetRefreshTokenByToken(
                request.RefreshToken,
                cancellationToken);
            if (refreshToken == null || refreshToken.ExpirationDate < DateTime.UtcNow)
                return new RefreshTokenResponse.InvalidRefreshToken("Invalid refresh token.");

            var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId, cancellationToken);
            var roles = new List<Role>();
            if (user == null)
                return new RefreshTokenResponse.NotFound("User not found.");

            await foreach (var role in _userRepository.GetUserRolesByIdAsync(user.Id, cancellationToken))
                roles.Add(role);

            var accessToken = _authTokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = _authTokenService.GenerateRefreshToken(user);

            using var transaction = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromSeconds(5)
                },
                TransactionScopeAsyncFlowOption.Enabled);

            await _refreshTokenRepository.AddRefreshTokenAsync(newRefreshToken, cancellationToken);
            await _refreshTokenRepository.RemoveRefreshTokenByTokenAsync(refreshToken.Token, cancellationToken);

            transaction.Complete();

            return new RefreshTokenResponse.Success(accessToken, newRefreshToken.Token);
        }
        catch (Exception e)
        {
            return new RefreshTokenResponse.ServerError(e.Message);
        }
    }
}