using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(product => product.Id);

            builder.Property(product => product.Name)
                .IsRequired()
                    .HasColumnType("varchar(200)");

            builder.Property(product => product.Description)
                .HasColumnType("varchar(400)");

            builder.Property(product => product.Image)
                .HasColumnType("varchar(100)");

            builder.Property(product => product.Value)
                .IsRequired()
                    .HasColumnType("decimal(18, 2)");

            builder.Property(product => product.CreationDate)
                .IsRequired()
                    .HasColumnType("datetime")
                        .HasDefaultValueSql("DATETIME('now')");

            builder.ToTable("Products", "dbo");
        }
    }
}
