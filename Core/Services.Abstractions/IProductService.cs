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
       Task<IEnumerable<ProductDto>> GetAllProductsAsync(int? brandId,int? typeId,string? sort,int pageIndex=1,int pageSize=5);

        //Get Product By Id
       Task<ProductDto?> GetProductByIdAsync(int id);

        //Get All Brand
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();

        //Get All Types
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
    }
}
