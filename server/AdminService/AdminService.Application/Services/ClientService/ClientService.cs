using AdminService.Application.Ports.Repositories;
using AdminService.Application.Services.ClientService.Requests;
using AdminService.Application.Services.ClientService.Responses;
using AdminService.Domain;

namespace AdminService.Application.Services.ClientService;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }


    public async Task<GetClientsResponse> GetClientsAsync(
        int limit,
        int? cursor,
        CancellationToken cancellationToken)
    {
        try
        {
            var clients = new List<Client>();
            await foreach (var client in _clientRepository.GetClientsAsync(limit, cursor, cancellationToken))
                clients.Add(client);

            return new GetClientsResponse.Success(clients);
        }
        catch (Exception e)
        {
            return new GetClientsResponse.ServerError(e.Message);
        }
    }

    public async Task<GetClientResponse> GetClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var client = await _clientRepository.GetClientByIdAsync(id, cancellationToken);
            if (client is null)
                return new GetClientResponse.NotFound();

            return new GetClientResponse.Success(client);
        }
        catch (Exception e)
        {
            return new GetClientResponse.ServerError(e.Message);
        }
    }

    public async Task<CreateClientResponse> CreateClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var client = await _clientRepository.CreateClientAsync(
                request.Name,
                request.Email,
                request.BalanceT,
                cancellationToken);

            return new CreateClientResponse.Success(client);
        }
        catch (Exception e)
        {
            return new CreateClientResponse.ServerError(e.Message);
        }
    }

    public async Task<UpdateClientResponse> UpdateClientAsync(Client client, CancellationToken cancellationToken)
    {
        try
        {
            var updatedClient = await _clientRepository.UpdateClientAsync(client, cancellationToken);
            if (updatedClient is null)
                return new UpdateClientResponse.NotFound();

            return new UpdateClientResponse.Success(updatedClient);
        }
        catch (Exception e)
        {
            return new UpdateClientResponse.ServerError(e.Message);
        }
    }

    public async Task<DeleteClientResponse> DeleteClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _clientRepository.DeleteClientByIdAsync(id, cancellationToken);

            return new DeleteClientResponse.Success();
        }
        catch (Exception e)
        {
            return new DeleteClientResponse.ServerError(e.Message);
        }
    }
}