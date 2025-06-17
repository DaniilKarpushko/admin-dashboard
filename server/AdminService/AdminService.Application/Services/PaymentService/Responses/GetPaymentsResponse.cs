using AdminService.Domain;

namespace AdminService.Application.Services.PaymentService.Responses;

public record GetPaymentsResponse
{
    public sealed record Success(IEnumerable<Payment> Payments) : GetPaymentsResponse;

    public sealed record ServerError(string Message) : GetPaymentsResponse;
};