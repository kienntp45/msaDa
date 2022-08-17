using MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.Samples.Services.Product.ProductPersistent.Database.EntityTypeConfiguration;


public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<ProductItem>
{
    public void Configure(EntityTypeBuilder<ProductItem> builder)
    {
        builder.ToTable("Tbl_Product");
        builder.HasKey(e=>e.Id).HasName("Tbl_Product_PK");
        builder.Property(e=>e.Id).HasColumnName("ProductId");
        builder.Property(e=>e.Name).HasColumnName("ProductName").HasMaxLength(200).IsRequired();
        builder.Property(e=>e.Price).HasColumnType("DECIMAL(18,2)").HasColumnName("ProductPrice");
        builder.Property(e=>e.AvailableQuantity).HasColumnName("AvailableQuantity");
    }
}