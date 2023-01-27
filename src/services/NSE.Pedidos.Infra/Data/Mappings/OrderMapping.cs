using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Pedidos.Domain.Orders;

namespace NSE.Pedidos.Infra.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(p => p.Address, e =>
            {
                e.Property(pe => pe.Street)
                    .HasColumnName("Street");

                e.Property(pe => pe.Number)
                    .HasColumnName("Number");

                e.Property(pe => pe.Complement)
                    .HasColumnName("Complement");

                e.Property(pe => pe.District)
                    .HasColumnName("District");

                e.Property(pe => pe.PostalCode)
                    .HasColumnName("PostalCode");

                e.Property(pe => pe.City)
                    .HasColumnName("City");

                e.Property(pe => pe.UF)
                    .HasColumnName("UF");
            });

            builder.Property(c => c.Code)
                .HasDefaultValueSql("NEXT VALUE FOR MySequence");

            // 1 : N => Pedido : PedidoItems
            builder.HasMany(c => c.OrderItems)
                .WithOne(c => c.Order)
                .HasForeignKey(c => c.OrderId);

            builder.ToTable("Orders");
        }
    }
}
