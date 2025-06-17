using AdminService.Api.Dtos;
using AdminService.Application.Services.ClientService.Requests;
using AdminService.Domain;

namespace AdminService.Api.Extensions;

public static class MappingExtensions
{
    public static CreateClientRequest ToModel(this CreateClientDto dto) => new(dto.Name, dto.Email, dto.BalanceT);

    public static Client ToModel(this UpdateClientDto dto, int id) =>
        new()
        {
            Id = id,
            Name = dto.Name,
            Email = dto.Email,
            BalanceT = dto.BalanceT,
        };
}