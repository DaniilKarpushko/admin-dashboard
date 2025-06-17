using System.Runtime.CompilerServices;
using AdminService.Application.Ports.Repositories;
using AdminService.Application.Services.PaymentService.Responses;
using AdminService.Domain;

namespace AdminService.Application.Services.PaymentService;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<GetPaymentsResponse> GetPaymentsAsync(
        int? limit,
        int? cursor,
        CancellationToken cancellationToken)
    {
        try
        {
            var payments = new List<Payment>();
            await foreach(var payment 
                          in _paymentRepository.GetPaymentsByClientIdAsync(limit, cursor, cancellationToken))
                payments.Add(payment);

            return new GetPaymentsResponse.Success(payments);
        }
        catch (Exception e)
        {
            return new GetPaymentsResponse.ServerError(e.Message);
        }
    }
}