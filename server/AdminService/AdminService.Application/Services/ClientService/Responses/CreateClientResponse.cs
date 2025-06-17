using AdminService.Domain;

namespace AdminService.Application.Services.ClientService.Responses;

public record CreateClientResponse
{
    public sealed record Success(Client Client) : CreateClientResponse;

    public sealed record ServerError(string ErrorMessage) : CreateClientResponse;
};