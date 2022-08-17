using MicroServices.Samples.Services.Basket.API.Application.Models;
using MicroServices.Samples.Services.Basket.API.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Basket.API.Database;


public class BasketDbConText : DbContext
{
    public BasketDbConText(DbContextOptions<BasketDbConText> options) : base(options) { }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<CustomerBasket> CustomerBaskets { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BasketItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerBasketEntityConfiguration());
    }
}