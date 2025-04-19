using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.ProductBrand)
                .WithMany()
                .HasForeignKey(P => P.BrandId);


            builder.HasOne(P => P.ProductType)
               .WithMany()
               .HasForeignKey(P => P.TypeId);

            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");

        }
    }
}
