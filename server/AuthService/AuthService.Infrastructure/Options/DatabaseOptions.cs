namespace AuthService.Infrastructure.Options;

public class DatabaseOptions
{
    public int Port { get; init; }
    public string Host { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    
    public string ConnectionString => $"Host={Host};Port={Port};Username={Username};Password={Password};";
}