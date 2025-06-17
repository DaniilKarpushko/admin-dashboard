using AdminService.Application.Services.PaymentService;
using AdminService.Application.Services.PaymentService.Responses;
using AdminService.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers;

[Route("api/payments")]
[ApiController]
[Authorize(Roles = "Admin")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult> GetPayments(
        [FromQuery] int? take,
        [FromQuery] int? cursor,
        CancellationToken cancellationToken)
    {
        var response = await _paymentService.GetPaymentsAsync(take, cursor, cancellationToken);

        return response switch
        {
            GetPaymentsResponse.ServerError serverError => StatusCode(500, serverError),
            GetPaymentsResponse.Success success => Ok(success),
            _ => throw new ArgumentOutOfRangeException(nameof(response))
        };
    }
}