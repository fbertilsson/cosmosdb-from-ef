using System.Diagnostics;
using CosmosdbFromEF.Model;
using Microsoft.EntityFrameworkCore;

namespace CosmosdbFromEF.Sql;

public class SqlPurchaseOrderRepository(PurchaseOrderContext ctx) : IPurchaseOrderRepository
{
    public async Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
    {
        var result = await ctx.AddAsync(purchaseOrder);
        await ctx.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByCustomerNameAsync(string customerName)
    {
        return await ctx.PurchaseOrders
            .Where(po => po.CustomerName == customerName)
            .ToListAsync();
    }

    public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
    {
        Debug.Assert(purchaseOrder.Id != null, "purchaseOrder.Id != null");
        ctx.PurchaseOrders.Update(purchaseOrder);
        await ctx.SaveChangesAsync();
    }

    public async Task DeletePurchaseOrderAsync(string id)
    {
        var toDelete = await ctx.PurchaseOrders.FindAsync(id);
        if (toDelete == null)
        {
            throw new InvalidOperationException($"Cannot delete: Purchase order not found. Id: {id}");
        }
        ctx.PurchaseOrders.Remove(toDelete);
        await ctx.SaveChangesAsync();
    }
}