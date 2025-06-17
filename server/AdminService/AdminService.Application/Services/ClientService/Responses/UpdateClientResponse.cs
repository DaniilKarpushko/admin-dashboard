using AdminService.Domain;

namespace AdminService.Application.Services.ClientService;

public record UpdateClientResponse
{
    public sealed record Success(Client Client) : UpdateClientResponse;
    public sealed record NotFound : UpdateClientResponse;
    public sealed record ServerError(string ErrorMessage) : UpdateClientResponse;
};