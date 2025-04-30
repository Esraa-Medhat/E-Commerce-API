using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Shared;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypesSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithBrandsAndTypesSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }
        public ProductWithBrandsAndTypesSpecifications(ProductSpecificationsParameters specParams, bool enablePaging = true) : base(
            P=>(!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId) &&
            (!specParams.TypeId.HasValue || P.TypeId == specParams.TypeId)

            )
        {

            ApplyIncludes();
            ApplySorting(specParams.Sort);
            if (enablePaging) {

                ApplyPagination(specParams.PageIndex, specParams.PageSize);
            }
               
           
          
        }
     

     
        private void ApplyIncludes()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(P => P.Name);
                        break;

                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }

            }
            else
            {
                AddOrderBy(P => P.Id);
            }

        }
    }
    
}
