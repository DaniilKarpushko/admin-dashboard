using Gateway.Api.Dtos;
using Gateway.Api.Extensions;
using Gateway.Grpc;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginService.LoginServiceClient _loginServiceClient;
    private readonly RefreshService.RefreshServiceClient _refreshServiceClient;

    public AuthController(
        LoginService.LoginServiceClient loginServiceClient,
        RefreshService.RefreshServiceClient refreshServiceClient)
    {
        _loginServiceClient = loginServiceClient;
        _refreshServiceClient = refreshServiceClient;
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto, CancellationToken cancellationToken)
    {
        var mappedRequest = requestDto.ToGrpc();
        var response = await _loginServiceClient.LoginAsync(mappedRequest, cancellationToken: cancellationToken);

        if (response.ResponseCase is not LoginResponse.ResponseOneofCase.Success)
            return response.Failure.Code switch
            {
                404 => NotFound(response.Failure.Message),
                401 => Unauthorized(response.Failure.Message),
                500 => StatusCode(500, response.Failure.Message),
                _ => throw new ArgumentOutOfRangeException(),
            };

        SetTokenCookies(response.Success.AccessToken, response.Success.RefreshToken);

        return Ok(new
        {
            response.Success.Email,
            response.Success.Username
        });
    }

    [Route("refresh")]
    [HttpPost]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var mappedRequest = requestDto.ToGrpc();
        var response = await _refreshServiceClient.RefreshAsync(mappedRequest, cancellationToken: cancellationToken);

        if (response.ResponseCase is not RefreshResponse.ResponseOneofCase.Success)
            return response.Failure.Code switch
            {
                401 => Unauthorized(response.Failure.Message),
                404 => NotFound(response.Failure.Message),
                505 => StatusCode(500, response.Failure.Message),
                _ => throw new ArgumentOutOfRangeException(nameof(response))
            };
        
        SetTokenCookies(response.Success.AccessToken, response.Success.RefreshToken);
        
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