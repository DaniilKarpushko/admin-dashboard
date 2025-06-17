using AuthService.Api.Dtos;
using AuthService.Api.Extensions;
using AuthService.Application.UseCases.LoginUseCase;
using AuthService.Application.UseCases.RefreshTokenUseCase;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILoginUseCase _loginUseCase;
    private readonly IRefreshTokenUseCase _refreshTokenUseCase;

    public AuthController(ILoginUseCase loginUseCase, IRefreshTokenUseCase refreshTokenUseCase)
    {
        _loginUseCase = loginUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto, CancellationToken cancellationToken)
    {
        var mappedRequest = requestDto.ToModel();
        var response = await _loginUseCase.ExecuteAsync(mappedRequest, cancellationToken);

        return response switch
        {
            LoginResponse.Success success => Ok(success),
            LoginResponse.NotFound notFound => NotFound(notFound),
            LoginResponse.InvalidPassword unauthorized => Unauthorized(unauthorized),
            LoginResponse.ServerError serverError => StatusCode(500, serverError),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    [Route("refresh")]
    [HttpPost]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var mappedRequest = requestDto.ToModel();
        var response = await _refreshTokenUseCase.ExecuteAsync(mappedRequest, cancellationToken);

        return response switch
        {
            RefreshTokenResponse.Success success => Ok(success),
            RefreshTokenResponse.InvalidRefreshToken invalidRefreshToken => Unauthorized(invalidRefreshToken),
            RefreshTokenResponse.NotFound notFound => NotFound(notFound),
            RefreshTokenResponse.ServerError serverError => StatusCode(500, serverError),
            _ => throw new ArgumentOutOfRangeException(nameof(response))
        };
    }
}