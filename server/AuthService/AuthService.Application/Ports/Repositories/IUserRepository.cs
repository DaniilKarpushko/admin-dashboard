using AuthService.Domain;

namespace AuthService.Application.Ports.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    IAsyncEnumerable<Role> GetUserRolesByIdAsync(Guid userId, CancellationToken cancellationToken);
}