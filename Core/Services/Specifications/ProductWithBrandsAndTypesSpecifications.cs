using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypesSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithBrandsAndTypesSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }
        public ProductWithBrandsAndTypesSpecifications(int? brandId, int? typeId) : base(
            P=>(!brandId.HasValue || P.BrandId == brandId)&&
            (!typeId.HasValue || P.TypeId == typeId)

            )
        {

            ApplyIncludes();
        }
     

     
        private void ApplyIncludes()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}
