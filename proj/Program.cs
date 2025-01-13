// See https://aka.ms/new-console-template for more information

using CosmosdbFromEF.Model;
using CosmosdbFromEF.Sql;

Console.WriteLine("Hello, World!");

var context = new PurchaseOrderContext();
context.Database.EnsureCreated();
var sqlRepo = new SqlPurchaseOrderRepository(context);

Dictionary<string, Part> parts = new()
{
    ["guitar"] = new Part { Id = "part1", Name = "guitar", Price = 10.0f },
    ["drum"] = new Part { Id = "part2", Name = "drum", Price = 20.0f },
    ["violin"] = new Part { Id = "part3", Name = "violin", Price = 30.0f },
};

var customer1Name = "John Doe";

var po1 = new PurchaseOrder
{
    CustomerName = customer1Name,
    ApprovedLimit = 70f,
    OrderLines =
    [
        new OrderLine { Quantity = 1, Price = 10.0f, PartId = parts["guitar"].Id! },
        new OrderLine { Quantity = 2, Price = 20.0f, PartId = parts["drum"].Id! },
        new OrderLine { Quantity = 3, Price = 30.0f, PartId = parts["violin"].Id! }
    ]
};

var readPo1 = await sqlRepo.CreatePurchaseOrderAsync(po1);

var orders = await sqlRepo.GetPurchaseOrdersByCustomerNameAsync(customer1Name);
Console.WriteLine($"Purchase orders in DB: {orders.Count()}");

await sqlRepo.DeletePurchaseOrderAsync(readPo1.Id!);