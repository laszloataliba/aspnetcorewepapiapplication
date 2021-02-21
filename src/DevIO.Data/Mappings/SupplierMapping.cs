using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings
{
    public class SupplierMapping : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(supplier => supplier.Id);

            builder.Property(supplier => supplier.Name)
                .IsRequired()
                    .HasColumnType("varchar(200)");

            builder.Property(supplier => supplier.IdentificationNumber)
                .HasColumnType("varchar(14)");

            builder.Property(supplier => supplier.Active)
                .HasColumnType("bit")
                    .HasDefaultValue(false);

            builder.HasOne(supplier => supplier.Address)
                .WithOne(address => address.Supplier);

            builder.HasMany(supplier => supplier.Products)
                .WithOne(product => product.Supplier)
                    .HasForeignKey(product => product.SupplierId);

            builder.ToTable("Suppliers", "dbo");
        }
    }
}
