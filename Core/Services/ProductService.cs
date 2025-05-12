using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork,IMapper mapper) : IProductService
    {
       

        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecificationsParameters specParams)
        {
            bool enablePaging = specParams.PageSize > 0;
            var spec = new ProductWithBrandsAndTypesSpecifications(specParams, enablePaging);
            //Get All Products through ProductRepository
            var products=await unitOfWork.GetRepository<Product, int >().GetAllAsync(spec);
            var specCount = new ProductWithCountSpecification(specParams);
            var count = await unitOfWork.GetRepository<Product, int>().CountAsync(specCount);
            //Mapping IEnumerable<Product> to IEnumerable<ProductDto>
           var result= mapper.Map<IEnumerable<ProductDto>>(products);
            return new PaginationResponse<ProductDto>(specParams.PageIndex,specParams.PageSize,count, result);

        }

  

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {

            var spec = new ProductWithBrandsAndTypesSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) throw new ProductNotFoundExceptions(id);
            var result = mapper.Map<ProductDto>(product);
            return result;
            
       
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
           var brands=await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
           var result= mapper.Map<IEnumerable<BrandDto>>(brands);
            return result;
        }

       

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
          var types= await  unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
           var result= mapper.Map<IEnumerable<TypeDto>>(types);
            return result;
        }

      
    }
}
