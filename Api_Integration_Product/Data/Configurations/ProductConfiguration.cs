using Api_Integration_Product.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api_Integration_Product.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(p => p.Stock)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
