using AdminService.Domain;

namespace AdminService.Application.Services.ClientService.Responses;

public record GetClientsResponse
{
    public sealed record Success(IEnumerable<Client> Clients) : GetClientsResponse;

    public sealed record ServerError(string Message) : GetClientsResponse;
};