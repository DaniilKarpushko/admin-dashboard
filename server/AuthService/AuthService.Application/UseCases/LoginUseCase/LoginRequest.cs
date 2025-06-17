namespace AuthService.Application.UseCases.LoginUseCase;

public record LoginRequest(string Email, string Password) : Request;