using AdminService.Application.Services.RateService.Requests;
using AdminService.Application.Services.RateService.Responses;
using AdminService.Domain;

namespace AdminService.Application.Services.RateService;

public interface IRateService
{
    public Task<GetRateResponse> GetRateAsync(GetRateRequest request, CancellationToken cancellationToken);
    public Task<UpdateRateResponse> UpdateRateAsync(decimal newRate, CancellationToken cancellationToken);
}