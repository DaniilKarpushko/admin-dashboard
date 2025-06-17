using AdminService.Application.Services.RateService;
using AdminService.Application.Services.RateService.Requests;
using AdminService.Application.Services.RateService.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers;

[Route("api/rates")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RateController : ControllerBase
{
    private readonly IRateService _rateService;

    public RateController(IRateService rateService)
    {
        _rateService = rateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRate(
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        CancellationToken cancellationToken)
    {
        var request = new GetRateRequest(from, to);
        var response = await _rateService.GetRateAsync(request, cancellationToken);
        return response switch
        {
            GetRateResponse.ServerError serverError => StatusCode(500, serverError),
            GetRateResponse.Success success => Ok(success),
            _ => throw new ArgumentOutOfRangeException(nameof(response))
        };
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateRate([FromQuery] decimal newRate, CancellationToken cancellationToken)
    {
        var response = await _rateService.UpdateRateAsync(newRate, cancellationToken);
        return response switch
        {
            UpdateRateResponse.ServerError serverError => StatusCode(500, serverError),
            UpdateRateResponse.Success success => Ok(success),
            _ => throw new ArgumentOutOfRangeException(nameof(response))
        };
    }
}