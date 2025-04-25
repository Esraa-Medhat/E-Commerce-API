using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) :ControllerBase
    {
        //endpoint: public non-static method
        [HttpGet] //Get  /api/Products
        public async Task<IActionResult> GetAllProducts(int? brandId,int? typeId)
        {
          var result=  await serviceManager.ProductService.GetAllProductsAsync(brandId,typeId);
            if (result is null) return BadRequest(); //400
            return Ok(result); //200

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
          var result= await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("brands")]
       public async Task<IActionResult> GetAllBrands()
        {
          var result= await  serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }
        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }

    }
}
