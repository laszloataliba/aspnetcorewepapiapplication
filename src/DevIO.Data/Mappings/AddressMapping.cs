using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(address => address.Id);

            builder.Property(address => address.StreetAddress)
                .IsRequired()
                    .HasColumnType("varchar(200)");

            builder.Property(address => address.Number)
                .IsRequired()
                    .HasColumnType("varchar(50)");

            builder.Property(address => address.ZipCode)
                .IsRequired()
                    .HasColumnType("varchar(8)");

            builder.Property(address => address.AddressAddOn)
                    .HasColumnType("varchar(250)");

            builder.Property(address => address.Neighborhood)
                .IsRequired()
                    .HasColumnType("varchar(100)");

            builder.Property(address => address.City)
                .IsRequired()
                    .HasColumnType("varchar(100)");

            builder.Property(address => address.State)
                .IsRequired()
                    .HasColumnType("varchar(50)");

            builder.ToTable("Addresses", "dbo");
        }
    }
}
