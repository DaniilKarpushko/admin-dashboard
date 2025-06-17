using System.Data;
using System.Runtime.CompilerServices;
using AdminService.Application.Ports.Repositories;
using AdminService.Domain;
using Npgsql;

namespace AdminService.Infrastructure;

public class ClientRepository : IClientRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ClientRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async IAsyncEnumerable<Client> GetClientsAsync(
        int limit,
        int? cursor,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var sql = """
                  select * from clients
                  where clients.id > :cursor
                  order by clients.id
                  limit :limit
                  """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("cursor", cursor ?? -1),
                new NpgsqlParameter("limit", limit),
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Client
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Email = reader.GetString("email"),
                BalanceT = reader.GetDecimal("balance_t"),
            };
        }
    }

    public async Task<Client> CreateClientAsync(
        string name,
        string email,
        decimal balanceT,
        CancellationToken cancellationToken)
    {
        const string sql = "insert into clients (name, email, balance_t) values (:name, :email, :balanceT) returning *";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("name", name),
                new NpgsqlParameter("email", email),
                new NpgsqlParameter("balanceT", balanceT),
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);

        return new Client
        {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("name"),
            Email = reader.GetString("email"),
            BalanceT = reader.GetDecimal("balance_t"),
        };
    }

    public async Task<Client?> GetClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = "select * from clients where clients.id = :id";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("id", id),
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new Client
        {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("name"),
            Email = reader.GetString("email"),
            BalanceT = reader.GetDecimal("balance_t"),
        };
    }

    public async Task<Client?> UpdateClientAsync(Client client, CancellationToken cancellationToken)
    {
        const string sql =
            "update clients set name = :name, email = :email, balance_t = :balanceT where id = :id returning *";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("name", client.Name),
                new NpgsqlParameter("email", client.Email),
                new NpgsqlParameter("balanceT", client.BalanceT),
                new NpgsqlParameter("id", client.Id),
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new Client
        {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("name"),
            Email = reader.GetString("email"),
            BalanceT = reader.GetDecimal("balance_t"),
        };
    }

    public async Task DeleteClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = "delete from clients where id = :id";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("id", id),
            }
        };

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}