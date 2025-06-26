using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoventiqAssignment.Services;
using NoventiqAssignment.Services.DTOModels;

namespace NoventiqAssignment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize(Roles = "Administrator")]
    //[Authorize(Roles = "User")]
    public class ProductController: ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product.ErrorList.Any())
                return BadRequest(product);

            return Ok(product);
        }
    }
}
