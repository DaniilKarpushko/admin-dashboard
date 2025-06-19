using System.ComponentModel.DataAnnotations;

namespace Gateway.Api.Dtos;

public class RefreshTokenRequestDto
{
    [Required]
    public string Token { get; set; } = string.Empty;
}