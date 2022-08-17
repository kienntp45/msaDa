using MicroServices.Samples.Services.Ordering.API.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.Samples.Services.Ordering.API.Database.EntityTypeConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("Tbl_OrderItem");
        builder.Property(e=>e.Id).HasColumnName("OrderItemId");
        builder.Property(e=>e.ProductId).HasColumnName("ProductId");
        builder.Property(e=>e.ProductName).HasColumnName("ProductName");
        builder.Property(e=>e.Quantity).HasColumnName("Quantity");
        builder.Property<int>("OrderId").IsRequired();
    }
}
