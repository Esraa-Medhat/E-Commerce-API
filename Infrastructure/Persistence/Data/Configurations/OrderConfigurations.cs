using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress,address=>address.WithOwner());
            builder.HasMany(O => O.orderItems)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(O => O.deliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(O => O.orderPaymentStatus)
                .HasConversion(s => s.ToString(), s => Enum.Parse<OrderPaymentStatus>(s));

            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,4)");
        }
    }
}

