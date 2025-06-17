using AdminService.Domain;

namespace AdminService.Application.Ports.Repositories;

public interface IClientRepository
{
    IAsyncEnumerable<Client> GetClientsAsync(int limit, int? cursor, CancellationToken cancellationToken);
    Task<Client> CreateClientAsync(string name, string email, decimal balanceT, CancellationToken cancellationToken);
    Task<Client?> GetClientByIdAsync(int id, CancellationToken cancellationToken);
    Task<Client?> UpdateClientAsync(Client client, CancellationToken cancellationToken);
    Task DeleteClientByIdAsync(int id, CancellationToken cancellationToken);
}