namespace AuthService.Domain;

public sealed class User
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Salt { get; init; } = string.Empty;
    public List<Role> Roles { get; set; } = [];
}