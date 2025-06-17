using NpgsqlTypes;

namespace AuthService.Domain;

public enum Role
{
    [PgName("Admin")]
    Admin,
    [PgName("User")]
    User,
}