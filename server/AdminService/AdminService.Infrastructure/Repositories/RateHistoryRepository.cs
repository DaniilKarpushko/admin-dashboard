using System.Data;
using System.Runtime.CompilerServices;
using AdminService.Application.Ports.Repositories;
using AdminService.Domain;
using Npgsql;
using NpgsqlTypes;

namespace AdminService.Infrastructure;

public class RateHistoryRepository : IRateHistoryRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public RateHistoryRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async IAsyncEnumerable<RateHistoryItem> GetRateHistoryAsync(
        DateTimeOffset? from,
        DateTimeOffset? to,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           (
                               select * from rate_history
                               where (:from::timestamp is null or created_at > :from)
                                 and (:to::timestamp is null or created_at < :to)
                               order by created_at desc
                           )
                           union all
                           (
                               select * from rate_history
                               where (:from::timestamp is not null and created_at <= :from)
                               order by created_at desc
                               limit 1
                           )
                           """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("from", NpgsqlDbType.TimestampTz)
                {
                    Value = (object?)from?.UtcDateTime ?? DBNull.Value
                },
                new NpgsqlParameter("to", NpgsqlDbType.TimestampTz)
                {
                    Value = (object?)to?.UtcDateTime ?? DBNull.Value
                }
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new RateHistoryItem
            {
                Rate = reader.GetDecimal("rate"),
                CreatedAt = reader.GetDateTime("created_at"),
            };
        }
    }

    public async Task CreateRateHistoryItemAsync(RateHistoryItem rateHistoryItem, CancellationToken cancellationToken)
    {
        const string sql = "insert into rate_history (rate, created_at) values (:rate, :createdAt) returning *";

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("rate", rateHistoryItem.Rate),
                new NpgsqlParameter("createdAt", rateHistoryItem.CreatedAt),
            }
        };

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}