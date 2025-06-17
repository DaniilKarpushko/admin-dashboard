using AdminService.Domain;

namespace AdminService.Application.Ports.Repositories;

public interface IRateHistoryRepository
{
    IAsyncEnumerable<RateHistoryItem> GetRateHistoryAsync(
        DateTimeOffset? from,
        DateTimeOffset? to,
        CancellationToken cancellationToken);

    Task CreateRateHistoryItemAsync(RateHistoryItem rateHistoryItem, CancellationToken cancellationToken);
}