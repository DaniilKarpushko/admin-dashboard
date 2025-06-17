namespace AdminService.Application.Services.RateService.Requests;

public record GetRateRequest(DateTimeOffset? From, DateTimeOffset? To);