using Gateway.Api.Dtos;
using Gateway.Grpc;

namespace Gateway.Api.Extensions;

public static class MappingExtensions
{
    public static LoginRequest ToGrpc(this LoginRequestDto requestDto) =>
        new()
        {
            Email = requestDto.Email,
            Password = requestDto.Password
        };
    
    public static RefreshRequest ToGrpc(this RefreshTokenRequestDto requestDto) =>
        new()
        {
            RefreshToken = requestDto.Token,
        };
}