using CosmosdbFromEF.Model;

namespace CosmosdbFromEF;

public interface IPurchaseOrderRepository
{
    public Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
    public Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByCustomerNameAsync(string customerName);
    public Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
    public Task DeletePurchaseOrderAsync(string id);
}