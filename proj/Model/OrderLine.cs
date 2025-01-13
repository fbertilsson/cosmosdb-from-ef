namespace CosmosdbFromEF.Model;

public record OrderLine
{
    public string? Id { get; set; }
    
    public int Quantity { get; set; }
    public float Price { get; set; }
    
    public required string PartId { get; set; }
}