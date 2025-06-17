using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdminService.Api.Dtos;

public class CreateClientDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DefaultValue(0)]
    [Range(0, double.MaxValue)]
    public decimal BalanceT { get; set; }
}