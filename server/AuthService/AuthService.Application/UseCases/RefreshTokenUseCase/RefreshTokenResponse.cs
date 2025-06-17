namespace AuthService.Application.UseCases.RefreshTokenUseCase;

public record RefreshTokenResponse : Response
{
    public sealed record Success(string Token, string RefreshToken) : RefreshTokenResponse();

    public sealed record NotFound(string ErrorMessage) : RefreshTokenResponse();

    public sealed record InvalidRefreshToken(string ErrorMessage) : RefreshTokenResponse();

    public sealed record ServerError(string ErrorMessage) : RefreshTokenResponse();
};