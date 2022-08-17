using Microsoft.EntityFrameworkCore;
using MicroServices.Samples.Services.Product.ProductPersistent.Database.EntityTypeConfiguration;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;

namespace MicroServices.Samples.Services.Product.ProductPersistent.Database;
public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options){}
    public DbSet<ProductItem> Products {get;set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
    }
}