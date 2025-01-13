namespace CosmosdbFromEF.Model;

public record PurchaseOrder
{
    public string? Id { get; set; }
    public string? CustomerName { get; set; }
    
    public float ApprovedLimit { get; set; }
    public List<OrderLine> OrderLines { get; set; } = [];
}