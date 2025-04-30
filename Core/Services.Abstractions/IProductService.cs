using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Services.Abstractions
{
    public interface IProductService
    {
        //Get All Product
       Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecificationsParameters
           specParams);

        //Get Product By Id
       Task<ProductDto?> GetProductByIdAsync(int id);

        //Get All Brand
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();

        //Get All Types
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
    }
}
