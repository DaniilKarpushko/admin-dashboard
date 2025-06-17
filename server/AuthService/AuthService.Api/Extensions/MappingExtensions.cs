using AuthService.Api.Dtos;
using AuthService.Application.UseCases.LoginUseCase;
using AuthService.Application.UseCases.RefreshTokenUseCase;

namespace AuthService.Api.Extensions;

public static class MappingExtensions
{
    public static LoginRequest ToModel(this LoginRequestDto requestDto) => new(requestDto.Email, requestDto.Password);

    public static RefreshTokenRequest ToModel(this RefreshTokenRequestDto requestDto) => new(requestDto.Token);
}