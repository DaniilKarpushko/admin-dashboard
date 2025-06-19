using AuthService.Application.UseCases.RefreshTokenUseCase;

namespace AuthService.Grpc.Extensions;

public static class MappingExtensions
{
    public static Application.UseCases.LoginUseCase.LoginRequest ToModel(this LoginRequest request) =>
        new(request.Email, request.Password);

    public static LoginResponse ToGrpc(this Application.UseCases.LoginUseCase.LoginResponse response)
    {
        return response switch
        {
            Application.UseCases.LoginUseCase.LoginResponse.InvalidPassword invalidPassword => new LoginResponse
            {
                Failure = new LoginFailure
                {
                    Code = 401,
                    Message = invalidPassword.ErrorMessage,
                }
            },
            Application.UseCases.LoginUseCase.LoginResponse.NotFound notFound => new LoginResponse
            {
                Failure = new LoginFailure
                {
                    Code = 404,
                    Message = notFound.ErrorMessage,
                }
            },
            Application.UseCases.LoginUseCase.LoginResponse.ServerError serverError => new LoginResponse
            {
                Failure = new LoginFailure
                {
                    Code = 500,
                    Message = serverError.ErrorMessage,
                }
            },
            Application.UseCases.LoginUseCase.LoginResponse.Success success => new LoginResponse
            {
                Success = new LoginSuccess
                {
                    AccessToken = success.AccessToken,
                    RefreshToken = success.RefreshToken,
                    Email = success.Email,
                    Username = success.Username,
                }
            },
            _ => throw new ArgumentOutOfRangeException(nameof(response))
        };
    }

    public static RefreshTokenRequest ToModel(this RefreshRequest request) =>
        new(request.RefreshToken);

    public static RefreshResponse ToGrpc(this RefreshTokenResponse response)
    {
        return response switch
        {
            RefreshTokenResponse.InvalidRefreshToken invalidRefreshToken => new RefreshResponse
            {
                Failure = new RefreshFailure
                {
                    Code = 401,
                    Message = invalidRefreshToken.ErrorMessage,
                }
            },
            RefreshTokenResponse.NotFound notFound => new RefreshResponse
            {
                Failure = new RefreshFailure
                {
                    Code = 404,
                    Message = notFound.ErrorMessage,
                }
            },
            RefreshTokenResponse.ServerError serverError => new RefreshResponse
            {
                Failure = new RefreshFailure
                {
                    Code = 500,
                    Message = serverError.ErrorMessage,
                }
            },
            RefreshTokenResponse.Success success => new RefreshResponse
            {
                Success = new RefreshSuccess
                {
                    AccessToken = success.Token,
                    RefreshToken = success.RefreshToken,
                }
            },
            _ => throw new ArgumentOutOfRangeException(nameof(response))
        };
    }
}