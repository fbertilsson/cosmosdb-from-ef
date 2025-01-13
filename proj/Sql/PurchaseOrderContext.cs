using CosmosdbFromEF.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CosmosdbFromEF.Sql;

public class PurchaseOrderContext : DbContext {
    private readonly SqliteConnection _connection;
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    private string DbPath { get; }

    public PurchaseOrderContext()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(path, "purchaseOrders.db");
        
        // _contextOptions = new DbContextOptionsBuilder<PurchaseOrderContext>()
        //     .UseSqlite(_connection)
        //     .Options;
        //
        // // Create the schema and seed some data
        // using var context = new PurchaseOrderContext(_contextOptions);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseOrder>()
            .Property(po => po.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<OrderLine>()
            .Property(po => po.Id)
            .ValueGeneratedOnAdd();
    }
    
    public override void Dispose()
    {
        _connection.Dispose();
        base.Dispose();
    }
}