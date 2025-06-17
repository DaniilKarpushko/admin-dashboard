namespace AuthService.Application.Ports.Services;

public interface IPasswordCoder
{
    public Task<string> EncodePasswordAsync(string password, out string salt);
    
    public Task<bool> CheckPasswordAsync(string password, string encodedPassword, string salt);
}