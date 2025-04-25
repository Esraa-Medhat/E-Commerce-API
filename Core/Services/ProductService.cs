using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork,IMapper mapper) : IProductService
    {
       

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(int? brandId, int? typeId)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(brandId,typeId);
            //Get All Products through ProductRepository
            var products=await unitOfWork.GetRepository<Product, int >().GetAllAsync(spec);

            //Mapping IEnumerable<Product> to IEnumerable<ProductDto>
           var result= mapper.Map<IEnumerable<ProductDto>>(products);
            return result;

        }

  

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {

            var spec = new ProductWithBrandsAndTypesSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) return null;
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
