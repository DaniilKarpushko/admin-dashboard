using System.Data;
using System.Runtime.CompilerServices;
using AuthService.Application.Ports.Repositories;
using AuthService.Domain;
using Npgsql;

namespace AuthService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = "select * from users where users.id =  :userId";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("userId", userId),
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new User
        {
            Id = reader.GetGuid("id"),
            Name = reader.GetString("name"),
            Email = reader.GetString("email"),
            Password = reader.GetString("password"),
            Salt = reader.GetString("salt"),
        };
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string sql = "select * from users where users.email =  :email";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("email", email),
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new User
        {
            Id = reader.GetGuid("id"),
            Name = reader.GetString("name"),
            Email = reader.GetString("email"),
            Password = reader.GetString("password"),
            Salt = reader.GetString("salt"),
        };
    }

    public async IAsyncEnumerable<Role> GetUserRolesByIdAsync(
        Guid userId,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           select * from user_roles
                           where user_id =  :userId
                           """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("userId", userId),
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return reader.GetFieldValue<Role>("role");
        }
    }
}