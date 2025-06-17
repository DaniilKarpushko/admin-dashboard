using AdminService.Api.Dtos;
using AdminService.Api.Extensions;
using AdminService.Application.Services.ClientService;
using AdminService.Application.Services.ClientService.Responses;
using AdminService.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize(Roles = "Admin")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClients(
        [FromQuery] int limit,
        [FromQuery] int? cursor,
        CancellationToken cancellationToken)
    {
        var response = await _clientService.GetClientsAsync(limit, cursor, cancellationToken);

        return response switch
        {
            GetClientsResponse.ServerError serverError => StatusCode(500, serverError),
            GetClientsResponse.Success success => Ok(success),
            _ => throw new ArgumentOutOfRangeException(nameof(response))
        };
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient(
        [FromBody] CreateClientDto clientDto,
        CancellationToken cancellationToken)
    {
        var response = await _clientService.CreateClientAsync(clientDto.ToModel(), cancellationToken);

        return response switch
        {
            CreateClientResponse.Success success => Ok(success),
            CreateClientResponse.ServerError serverError => StatusCode(500, serverError),
            _ => throw new ArgumentOutOfRangeException(nameof(response)),
        };
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetClientById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var response = await _clientService.GetClientByIdAsync(id, cancellationToken);

        return response switch
        {
            GetClientResponse.Success success => Ok(success),
            GetClientResponse.NotFound notFound => NotFound(notFound),
            GetClientResponse.ServerError serverError => StatusCode(500, serverError),
            _ => throw new ArgumentOutOfRangeException(nameof(response)),
        };
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateClient(
        [FromRoute] int id,
        [FromBody] UpdateClientDto clientDto,
        CancellationToken cancellationToken)
    {
        var response = await _clientService.UpdateClientAsync(clientDto.ToModel(id), cancellationToken);

        return response switch
        {
            UpdateClientResponse.Success success => Ok(success),
            UpdateClientResponse.NotFound notFound => NotFound(notFound),
            UpdateClientResponse.ServerError serverError => StatusCode(500, serverError),
            _ => throw new ArgumentOutOfRangeException(nameof(response)),
        };
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteClient(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var response = await _clientService.DeleteClientByIdAsync(id, cancellationToken);

        return response switch
        {
            DeleteClientResponse.Success success => Ok(success),
            DeleteClientResponse.ServerError serverError => StatusCode(500, serverError),
            _ => throw new ArgumentOutOfRangeException(nameof(response)),
        };
    }
}