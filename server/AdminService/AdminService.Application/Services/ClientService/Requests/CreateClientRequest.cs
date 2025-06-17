namespace AdminService.Application.Services.ClientService.Requests;

public record CreateClientRequest(string Name, string Email, decimal BalanceT);