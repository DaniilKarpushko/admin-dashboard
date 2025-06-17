using AuthService.Application.Ports.Repositories;
using AuthService.Application.Ports.Services;
using AuthService.Application.UseCases;
using AuthService.Application.UseCases.LoginUseCase;
using AuthService.Domain;
using AuthService.Infrastructure.Options;
using AuthService.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace AuthService.Tests;

public class LoginUseCaseTests
{
    private readonly ILoginUseCase _loginUseCase;

    public LoginUseCaseTests()
    {
        var passwordOptionsMoq = new Mock<IOptions<PasswordOptions>>();
        passwordOptionsMoq.Setup(x => x.Value).Returns(new PasswordOptions()
        {
            Iterations = 1,
            KeySize = 256,
            SaltSize = 128
        });
        IPasswordCoder passwordCoder = new PasswordCoder(passwordOptionsMoq.Object);

        var testUser = new User()
        {
            Email = "email",
            Name = "name",
            Password = passwordCoder.EncodePasswordAsync("password", out var salt).Result,
            Salt = salt,
            Id = Guid.NewGuid()
        };

        Mock<IUserRepository> authRepositoryMoq = new Mock<IUserRepository>();
        authRepositoryMoq.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(testUser));
        authRepositoryMoq.Setup(x => x.GetUserRolesByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(() => GetRolesMock());

        var tokenOptionsMoq = new Mock<IOptions<JwtTokenOptions>>();
        tokenOptionsMoq.Setup(x => x.Value).Returns(new JwtTokenOptions()
        {
            Key = "keykeykeykeykeykeykeykeykeykeykeykeykey",
            AccessTokenExpiration = 1,
            RefreshTokenExpiration = 1
        });

        IAuthTokenService authTokenService = new AuthTokenService(tokenOptionsMoq.Object);

        Mock<IRefreshTokenRepository> refreshTokenRepositoryMoq = new Mock<IRefreshTokenRepository>();

        _loginUseCase = new LoginUseCase(
            authRepositoryMoq.Object,
            refreshTokenRepositoryMoq.Object,
            authTokenService,
            passwordCoder);
    }

    [Fact]
    public async Task LoginUseCase_ValidInput_ShouldReturnSuccess()
    {
        var loginRequest = new LoginRequest("email", "password");
        var response = await _loginUseCase.ExecuteAsync(loginRequest, CancellationToken.None);
        Assert.IsType<LoginResponse.Success>(response);
    }

    private async IAsyncEnumerable<Role> GetRolesMock()
    {
        yield return Role.Admin;
    }
}