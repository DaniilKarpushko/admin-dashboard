using AuthService.Application.UseCases.RefreshTokenUseCase;
using AuthService.Grpc.Extensions;
using Grpc.Core;

namespace AuthService.Grpc.Services;

public class RefreshService : Grpc.RefreshService.RefreshServiceBase
{
    private readonly IRefreshTokenUseCase _refreshTokenUseCase;

    public RefreshService(IRefreshTokenUseCase refreshTokenUseCase)
    {
        _refreshTokenUseCase = refreshTokenUseCase;
    }

    public override async Task<RefreshResponse> Refresh(RefreshRequest request, ServerCallContext context)
    {
        var mappedRequest = request.ToModel();
        var response = await _refreshTokenUseCase.ExecuteAsync(mappedRequest, context.CancellationToken);

        return response.ToGrpc();
    }
}