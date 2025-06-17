using System.Data;
using System.Runtime.CompilerServices;
using AdminService.Application.Ports.Repositories;
using AdminService.Domain;
using Npgsql;

namespace AdminService.Infrastructure;

public class PaymentRepository : IPaymentRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PaymentRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async IAsyncEnumerable<Payment> GetPaymentsByClientIdAsync(
        int? limit,
        int? cursor,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           select p.id, p.total, p.created_at, p.client_id, c.name from payments as p
                           join clients as c on p.client_id = c.id
                           where p.id >= :cursor
                           order by p.created_at desc, p.id
                           limit :limit
                           """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("limit", limit ?? 1),
                new NpgsqlParameter("cursor", cursor ?? 0),
            }
        };
        
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Payment
            {
                Id = reader.GetInt32("id"),
                Total = reader.GetDecimal("total"),
                CreatedAt = reader.GetDateTime("created_at"),
                ClientId = reader.GetInt32("client_id"),
                ClientName = reader.GetString("name")
            };
        }
    }
}