using AdminService.Application.Services.PaymentService.Responses;
using AdminService.Domain;

namespace AdminService.Application.Services.PaymentService;

public interface IPaymentService
{
    Task<GetPaymentsResponse> GetPaymentsAsync(int? limit, int? cursor, CancellationToken cancellationToken);
}