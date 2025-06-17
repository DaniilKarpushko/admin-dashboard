using AdminService.Application.Ports.Repositories;
using AdminService.Application.Services.RateService.Requests;
using AdminService.Application.Services.RateService.Responses;
using AdminService.Domain;

namespace AdminService.Application.Services.RateService;

public class RateService : IRateService
{
    private readonly IRateHistoryRepository _rateRepository;

    public RateService(IRateHistoryRepository rateRepository)
    {
        _rateRepository = rateRepository;
    }

    public async Task<GetRateResponse> GetRateAsync(GetRateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var history = new List<RateHistoryItem>();

            await foreach (var item in _rateRepository.GetRateHistoryAsync(request.From, request.To, cancellationToken))
                history.Add(item);

            return new GetRateResponse.Success(history);
        }
        catch (Exception e)
        {
            return new GetRateResponse.ServerError(e.Message);
        }
    }

    public async Task<UpdateRateResponse> UpdateRateAsync(decimal newRate, CancellationToken cancellationToken)
    {
        try
        {
            var newRateHistoryItem = new RateHistoryItem { Rate = newRate, CreatedAt = DateTimeOffset.UtcNow };
            await _rateRepository.CreateRateHistoryItemAsync(newRateHistoryItem, cancellationToken);

            return new UpdateRateResponse.Success();
        }
        catch (Exception e)
        {
            return new UpdateRateResponse.ServerError(e.Message);
        }
    }
}