namespace AdminService.Application.Services.ClientService.Responses;

public record DeleteClientResponse
{
    public record Success : DeleteClientResponse;
    public record ServerError(string Message) : DeleteClientResponse;
};