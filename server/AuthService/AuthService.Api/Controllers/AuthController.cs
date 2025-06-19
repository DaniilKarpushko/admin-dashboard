using AuthService.Api.Dtos;
using AuthService.Api.Extensions;
using AuthService.Application.UseCases;
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

        if (response is not LoginResponse.Success success)
            return response switch
            {
                LoginResponse.NotFound notFound => NotFound(notFound),
                LoginResponse.InvalidPassword unauthorized => Unauthorized(unauthorized),
                LoginResponse.ServerError serverError => StatusCode(500, serverError),
                _ => throw new ArgumentOutOfRangeException(),
            };

        SetTokenCookies(success.AccessToken, success.RefreshToken);
        return Ok(new
        {
            success.Username,
            success.Email,
        });
    }

    [Route("refresh")]
    [HttpPost]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken))
            return BadRequest();

        var request = new RefreshTokenRequest(refreshToken);
        var response = await _refreshTokenUseCase.ExecuteAsync(request, cancellationToken);

        if (response is not RefreshTokenResponse.Success success)
            return response switch
            {
                RefreshTokenResponse.InvalidRefreshToken invalidRefreshToken => Unauthorized(invalidRefreshToken),
                RefreshTokenResponse.NotFound notFound => NotFound(notFound),
                RefreshTokenResponse.ServerError serverError => StatusCode(500, serverError),
                _ => throw new ArgumentOutOfRangeException(nameof(response))
            };

        SetTokenCookies(success.Token, success.RefreshToken);
        return Ok();
    }

    private void SetTokenCookies(string accessToken, string refreshToken)
    {
        Response.Cookies.Append("X-Access-Token", accessToken, new CookieOptions()
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        });
        Response.Cookies.Append("X-Refresh-Token", refreshToken, new CookieOptions()
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(30)
        });
    }
}