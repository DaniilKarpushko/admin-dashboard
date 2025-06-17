using AdminService.Domain;

namespace AdminService.Application.Services.RateService.Responses;

public record UpdateRateResponse
{
    public sealed record Success() : UpdateRateResponse;

    public sealed record ServerError(string Message) : UpdateRateResponse;
};