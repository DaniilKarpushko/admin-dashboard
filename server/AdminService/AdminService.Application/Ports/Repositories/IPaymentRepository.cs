using AdminService.Domain;

namespace AdminService.Application.Ports.Repositories;

public interface IPaymentRepository
{
    IAsyncEnumerable<Payment> GetPaymentsByClientIdAsync(int? limit,
        int? cursor,
        CancellationToken cancellationToken);
}