﻿namespace AdminService.Domain;

public class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    
    public decimal BalanceT { get; set; }
}