using MicroServices.Samples.Services.Product.ProductPersistent.Database;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Product.ProductPersistent.Factory;

public class ProductDbContextFactory : IDbContextFactory<ProductDbContext>
{
    private readonly string _connectionString;

    public ProductDbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public ProductDbContext CreateDbContext()
    {
       var dbContextBuilder = new DbContextOptionsBuilder<ProductDbContext>().UseOracle(_connectionString);
       var dbContext = new ProductDbContext(dbContextBuilder.Options);
       dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
       return dbContext;
    }
}