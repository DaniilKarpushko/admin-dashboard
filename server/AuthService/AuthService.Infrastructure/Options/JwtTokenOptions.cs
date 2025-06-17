namespace AuthService.Infrastructure.Options;

public class JwtTokenOptions
{
    public string Key { get; set; } = string.Empty;
    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }
}