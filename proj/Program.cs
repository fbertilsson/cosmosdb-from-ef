// See https://aka.ms/new-console-template for more information

using CosmosdbFromEF;
using CosmosdbFromEF.CosmosNoSql;
using CosmosdbFromEF.Model;
using CosmosdbFromEF.Sql;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello there!");

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
var config = builder.Build();

var useSql = args.Length > 0 && args[0].Equals("sql", StringComparison.OrdinalIgnoreCase);

IPurchaseOrderRepository repo;
if (useSql)
{
    var context = new PurchaseOrderContext();
    context.Database.EnsureCreated();
    repo = new SqlPurchaseOrderRepository(context);
}
else
{
    repo = new CosmosPurchaseOrderRepository(config);
}

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

var readPo1 = await repo.CreatePurchaseOrderAsync(po1);

var orders = await repo.GetPurchaseOrdersByCustomerNameAsync(customer1Name);
Console.WriteLine($"Purchase orders in DB: {orders.Count()}");

await repo.DeletePurchaseOrderAsync(readPo1.Id!);