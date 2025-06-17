namespace AdminService.Domain;

public class Payment
{
    public int Id { get; set; }
    
    public int ClientId { get; set; }

    public string ClientName { get; set; } = string.Empty;
    
    public decimal Total { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
}