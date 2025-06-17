namespace AuthService.Domain;

public sealed class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    
    public DateTime ExpirationDate { get; set; }

    public Guid UserId { get; set; }
} 