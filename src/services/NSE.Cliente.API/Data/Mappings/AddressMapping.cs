using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Cliente.API.Models;

namespace NSE.Cliente.API.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PublicPlace)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Number)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(x => x.Cep)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(x => x.Complement)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(x => x.District)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(x => x.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(x => x.State)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Addresses");
        }
    }
}
