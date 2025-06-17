using System.Data;
using AuthService.Domain;
using AuthService.Application.Ports.Repositories;
using Npgsql;
using NpgsqlTypes;

namespace AuthService.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public RefreshTokenRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<RefreshToken?> GetRefreshTokenByToken(string token, CancellationToken cancellationToken)
    {
        const string sql = "select * from refresh_tokens where token = :token";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("token", token)
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new RefreshToken
        {
            Token = reader.GetString("token"),
            ExpirationDate = reader.GetDateTime("expiration_date"),
            UserId = reader.GetGuid("user_id")
        };
    }

    public async Task<RefreshToken?> GetTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = "select * from refresh_tokens where user_id = :user_id";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("user_id", userId)
            }
        };

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        return new RefreshToken
        {
            Token = reader.GetString("token"),
            ExpirationDate = reader.GetDateTime("expiration_date"),
            UserId = reader.GetGuid("user_id")
        };
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        const string sql = """
                           insert into refresh_tokens (token, expiration_date, user_id)
                           values (:token, :expiration_date, :user_id)
                           """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("token", refreshToken.Token),
                new NpgsqlParameter("expiration_date", refreshToken.ExpirationDate),
                new NpgsqlParameter("user_id", refreshToken.UserId)
            }
        };

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task RemoveRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken)
    {
        const string sql = "delete from refresh_tokens where token = :token";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("token", token),
            }
        };

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}