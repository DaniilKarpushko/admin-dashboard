using AdminService.Application.Services.ClientService.Requests;
using AdminService.Application.Services.ClientService.Responses;
using AdminService.Domain;

namespace AdminService.Application.Services.ClientService;

public interface IClientService
{
    Task<GetClientsResponse> GetClientsAsync(int limit, int? cursor, CancellationToken cancellationToken);
    Task<GetClientResponse> GetClientByIdAsync(int id, CancellationToken cancellationToken);
    Task<CreateClientResponse> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken);
    Task<UpdateClientResponse> UpdateClientAsync(Client client, CancellationToken cancellationToken);
    Task<DeleteClientResponse> DeleteClientByIdAsync(int id, CancellationToken cancellationToken);
}