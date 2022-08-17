using Microsoft.EntityFrameworkCore;
using MicroServices.Samples.Services.Product.API.Application.Models;
using MicroServices.Samples.Services.Product.API.Database.EntityTypeConfiguration;

namespace MicroServices.Samples.Services.Product.API.Database;
public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options){}
    public DbSet<ProductItem> Products {get;set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
    }
}