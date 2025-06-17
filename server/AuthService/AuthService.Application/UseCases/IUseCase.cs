namespace AuthService.Application.UseCases;

public interface IUseCase<TRequest, TResponse> 
    where TRequest : Request
    where TResponse : Response
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}