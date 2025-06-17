using AdminService.Domain;

namespace AdminService.Application.Services.RateService.Responses;

public record GetRateResponse
{
    public sealed record Success(List<RateHistoryItem> Rates) : GetRateResponse;

    public sealed record NotFound : GetRateResponse;

    public sealed record ServerError(string Message) : GetRateResponse;
};