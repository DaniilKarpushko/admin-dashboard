namespace AdminService.Infrastructure.Options;

public class DatabaseOptions
{
    public int Port { get; set; }
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string ConnectionString => $"Host={Host};Port={Port};Username={Username};Password={Password};";
}