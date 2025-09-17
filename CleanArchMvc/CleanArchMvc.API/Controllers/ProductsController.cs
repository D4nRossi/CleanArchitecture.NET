using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var categories = await _productService.GetProductsAsync();
            if (categories == null)
                return NotFound("Categories not found");
            return Ok(categories);
        }

        [HttpGet(( "{id}" ), Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var category = await _productService.GetByIdAsync(id);
            if (category == null)
                return NotFound("Category not found");
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest("Invalid Data");

            await _productService.CreateAsync(productDTO);

            return new CreatedAtRouteResult("GetProduct", new { id = productDTO.Id }, productDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
                return BadRequest();

            if (productDTO == null)
                return BadRequest();

            await _productService.UpdateAsync(productDTO);
            return Ok(productDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound("Category not found");

            await _productService.RemoveAsync(id);
            return Ok(product);
        }

    }
}
