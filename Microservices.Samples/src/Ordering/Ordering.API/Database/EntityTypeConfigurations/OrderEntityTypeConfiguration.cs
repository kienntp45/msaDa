using MicroServices.Samples.Services.Ordering.API.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.Samples.Services.Ordering.API.Database.EntityTypeConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Tbl_Order");
        builder
            .HasKey(e => e.Id)
            .HasName("Tbl_OrderId_PK");
        builder.Property(e => e.Id).HasColumnName("OrderId");
        builder.Property(e => e.OrderDate).HasColumnName("OrderDate");
        builder.Property(e => e.Street).HasColumnName("Street");
        builder.Property(e => e.District).HasColumnName("District");
        builder.Property(e => e.City).HasColumnName("City");
        builder.Property(e => e.AdditionalAddress).HasColumnName("AdditionalAddress");
        builder.Property(e => e.CustomerId).HasColumnName("CustomerId");
        builder.HasOne<Customer>()
        .WithMany()
        .IsRequired(false)
        .HasForeignKey(e => e.CustomerId);

        var navigation = builder.Metadata.FindNavigation(nameof(Order.Items));
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
