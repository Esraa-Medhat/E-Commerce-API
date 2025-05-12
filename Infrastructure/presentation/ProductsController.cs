using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ErrorsModels;

namespace presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) :ControllerBase
    {
        //endpoint: public non-static method

        //sort
        //based on 1-name asc [default]
        //2- name desc
        //3- price asc
        //4- price desc


        [HttpGet] //Get  /api/Products
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(PaginationResponse<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type =typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(ErrorDetails))]
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery]ProductSpecificationsParameters specParams )
        {
          var result=  await serviceManager.ProductService.GetAllProductsAsync(specParams);
            
            return Ok(result); //200

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
          var result= await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<BrandDto>> GetAllBrands()
        {
          var result= await  serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }
        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<TypeDto>> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }

    }
}
