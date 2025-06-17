using System.Security.Cryptography;
using System.Text;
using AuthService.Application.Ports.Services;
using AuthService.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace AuthService.Infrastructure.Services;

public class PasswordCoder : IPasswordCoder
{
    private readonly PasswordOptions _options;

    public PasswordCoder(IOptions<PasswordOptions> options)
    {
        _options = options.Value;
    }

    public Task<string> EncodePasswordAsync(string password, out string salt)
    {
        var byteSalt = RandomNumberGenerator.GetBytes(_options.SaltSize);
        salt = Convert.ToBase64String(byteSalt);
        var hash = EncodePassword(password, byteSalt);

        return Task.FromResult(Convert.ToHexString(hash));
    }

    public Task<bool> CheckPasswordAsync(string password, string encodedPassword, string salt)
    {
        var encodedPasswordToCheck = EncodePassword(password, Convert.FromBase64String(salt));
        Console.WriteLine($"Encoded (hex): {Convert.ToHexString(encodedPasswordToCheck)}");
        return Task.FromResult(encodedPassword == Convert.ToHexString(encodedPasswordToCheck));
    }
    
    private byte[] EncodePassword(string password, byte[] salt) =>
        Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            _options.Iterations,
            HashAlgorithmName.SHA512,
            _options.KeySize);
    
}