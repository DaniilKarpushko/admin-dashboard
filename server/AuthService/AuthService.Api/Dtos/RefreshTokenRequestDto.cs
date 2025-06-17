using System.ComponentModel.DataAnnotations;

namespace AuthService.Api.Dtos;

public class RefreshTokenRequestDto
{
    [Required]
    public string Token { get; set; } = string.Empty;
}