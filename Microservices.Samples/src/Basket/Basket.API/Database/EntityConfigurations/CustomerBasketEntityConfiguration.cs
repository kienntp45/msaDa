using MicroServices.Samples.Services.Basket.API.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.Samples.Services.Basket.API.Database.EntityConfigurations;



public class CustomerBasketEntityConfiguration : IEntityTypeConfiguration<CustomerBasket>
{
    public void Configure(EntityTypeBuilder<CustomerBasket> builder)
    {
        builder.ToTable("Tbl_CustomerBasket");
        builder.HasKey(c => c.CusTomerId).HasName("Tbl_CustomerBasket_Id");
        builder.Property(c => c.CusTomerId).HasColumnName("CustomerId");

        var navigation = builder.Metadata.FindNavigation(nameof(CustomerBasket.Items));
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
