using MicroServices.Samples.Services.Ordering.API.Application.Models;
using MicroServices.Samples.Services.Ordering.API.Database.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Ordering.API.Database;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    public DbSet<Customer> Customers {get;set;}
    public DbSet<Order> Orders {get;set;}
    public DbSet<OrderItem> OrderItems{get;set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
    }
}