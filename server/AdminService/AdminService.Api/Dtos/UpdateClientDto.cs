using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdminService.Api.Dtos;

public class UpdateClientDto
{
    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    [DefaultValue(0)]
    [Range(0, double.MaxValue)]
    public decimal BalanceT { get; init; }
}