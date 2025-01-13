namespace CosmosdbFromEF.Model;

public record Part
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public float Price { get; set; }
}