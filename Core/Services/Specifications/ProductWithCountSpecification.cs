using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Shared;

namespace Services.Specifications
{
    internal class ProductWithCountSpecification:BaseSpecifications<Product,int>
    {
        public ProductWithCountSpecification(ProductSpecificationsParameters specParams) :base(

                 P => (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId) &&
            (!specParams.TypeId.HasValue || P.TypeId == specParams.TypeId)

            )
        {
            
        }
    }
}
