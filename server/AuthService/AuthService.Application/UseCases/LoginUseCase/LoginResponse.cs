namespace AuthService.Application.UseCases.LoginUseCase;

public record LoginResponse : Response
{
    public sealed record Success(
        string Username,
        string Email,
        string AccessToken,
        string RefreshToken) : LoginResponse;

    public sealed record NotFound(string ErrorMessage) : LoginResponse;
    
    public sealed record InvalidPassword(string ErrorMessage) : LoginResponse;

    public sealed record ServerError(string ErrorMessage) : LoginResponse;
};