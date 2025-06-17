using AdminService.Domain;

namespace AdminService.Application.Services.ClientService;

public record GetClientResponse
{
    public sealed record Success(Client Client) : GetClientResponse;

    public sealed record NotFound : GetClientResponse;

    public sealed record ServerError(string ErrorMessage) : GetClientResponse;
};