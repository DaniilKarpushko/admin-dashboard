using AuthService.Application.Ports.Repositories;
using AuthService.Application.Ports.Services;
using AuthService.Domain;

namespace AuthService.Application.UseCases.LoginUseCase;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private readonly IAuthTokenService _authTokenService;
    private readonly IPasswordCoder _passwordEncoder;

    public LoginUseCase(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IAuthTokenService authTokenService,
        IPasswordCoder passwordEncoder)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _authTokenService = authTokenService;
        _passwordEncoder = passwordEncoder;
    }

    public async Task<LoginResponse> ExecuteAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user == null)
                return new LoginResponse.NotFound("User not found.");

            var isValidPassword =
                await _passwordEncoder.CheckPasswordAsync(request.Password, user.Password, user.Salt);
            if (!isValidPassword)
                return new LoginResponse.InvalidPassword("Invalid password.");

            var roles = new List<Role>();
            await foreach (var role in _userRepository.GetUserRolesByIdAsync(user.Id, cancellationToken))
                roles.Add(role);

            var token = _authTokenService.GenerateAccessToken(user, roles);
            var refreshToken = _authTokenService.GenerateRefreshToken(user);

            await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

            return new LoginResponse.Success(user.Name, user.Email, token, refreshToken.Token);
        }
        catch (Exception e)
        {
            return new LoginResponse.ServerError(e.Message);
        }
    }
}