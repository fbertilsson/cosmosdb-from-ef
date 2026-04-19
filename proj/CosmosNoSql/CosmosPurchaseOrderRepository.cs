using CosmosdbFromEF.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CosmosdbFromEF.CosmosNoSql;

public class CosmosPurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly CosmosClient _client;

    public CosmosPurchaseOrderRepository(IConfiguration config)
    {
        _client = new CosmosClient(
            accountEndpoint: config["Cosmos:EndpointUri"], 
            authKeyOrResourceToken: config["Cosmos:AccountKey"]
        );
    }
    
    
    public async Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
    {
        purchaseOrder.Id = Guid.NewGuid().ToString();
        var response = await GetContainer().CreateItemAsync(purchaseOrder);
        Console.WriteLine($"Create: [{response.StatusCode}]\t{purchaseOrder.CustomerName}\t{response.RequestCharge} RUs \tClientElapsedTime: {response.Diagnostics.GetClientElapsedTime().TotalMilliseconds}");
        return response.Resource;
    }

    public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByCustomerNameAsync(string customerName)
    {
        var sql = "select * from c where c.CustomerName = @customerName";
        QueryDefinition queryDefinition = new QueryDefinition(sql)
            .WithParameter("@customerName", customerName);
        using var feedIterator = GetContainer().GetItemQueryIterator<PurchaseOrder>(queryDefinition);
        var response = await feedIterator.ReadNextAsync();
        Console.WriteLine($"GetByCustomerName: [{response.StatusCode}]\t{customerName}\t{response.RequestCharge} RUs \tClientElapsedTime: {response.Diagnostics.GetClientElapsedTime().TotalMilliseconds}");
        return response.Resource;
    }

    public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
    {
        var response = await GetContainer().ReplaceItemAsync(
            purchaseOrder,
            purchaseOrder.Id,
            new PartitionKey(purchaseOrder.Id));
        Console.WriteLine($"Update: [{response.StatusCode}]\t{purchaseOrder.CustomerName}\t{response.RequestCharge} RUs \tClientElapsedTime: {response.Diagnostics.GetClientElapsedTime().TotalMilliseconds}");
    }

    public async Task DeletePurchaseOrderAsync(string id)
    {
        var response = await GetContainer().DeleteItemAsync<PurchaseOrder>(
            id,
            new PartitionKey(id));
        Console.WriteLine($"Delete: [{response.StatusCode}]\t{id}\t{response.RequestCharge} RUs \tClientElapsedTime: {response.Diagnostics.GetClientElapsedTime().TotalMilliseconds}");
    }

    private Container GetContainer()
    {
        var database = _client.GetDatabase("OrderingSystemDb");
        return database.GetContainer("purchaseOrders");
    }
}