using AuthService.Application.UseCases.LoginUseCase;
using AuthService.Grpc.Extensions;
using Grpc.Core;

namespace AuthService.Grpc.Services;

public class LoginService : Grpc.LoginService.LoginServiceBase
{
    private readonly ILoginUseCase _loginUseCase;

    public LoginService(ILoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var mappedRequest = request.ToModel();
        var response = await _loginUseCase.ExecuteAsync(mappedRequest, context.CancellationToken);

        return response.ToGrpc();
    }
}