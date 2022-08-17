using MicroServices.Samples.Services.Ordering.API.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.Samples.Services.Ordering.API.Database.EntityTypeConfigurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Tbl_Customer");
        builder.HasKey(e=>e.Id).HasName("Tbl_CustomerId_PK");
        builder.Property(e=>e.Id).HasColumnName("CustomerId");
        builder.Property(e=>e.IdentityId).HasColumnName("IdentityId");
        builder.Property(e=>e.Name).HasColumnName("CustomerName");
        builder.Property(e=>e.PhoneNumber).HasColumnName("CustomerPhoneNumber"); 
    }
}
