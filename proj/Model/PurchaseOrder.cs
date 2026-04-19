using Newtonsoft.Json;

namespace CosmosdbFromEF.Model;

public record PurchaseOrder
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    public string? CustomerName { get; set; }
    
    public float ApprovedLimit { get; set; }
    public List<OrderLine> OrderLines { get; set; } = [];
}